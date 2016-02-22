using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using LINQPad.Extensibility.DataContext;
using Microsoft.CSharp;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using OData4.Builder;
using OData4.UI;

namespace OData4
{
    public class OData4DynamicDriver : DynamicDataContextDriver
    {
        /// <summary> System assemblies, using to compile generated odata client code </summary>
        private static readonly string[] SystemAssemblies =
        {
            "System.dll",
            "System.Core.dll",
            "System.Xml.dll"
        };

        /// <summary> Assemblies, using both to compile generated code and to load into LINQPad </summary>
        private static readonly string[] Assemblies = {
            "Microsoft.OData.Client.dll",
            "Microsoft.OData.Core.dll",
            "Microsoft.OData.Edm.dll",
            "Microsoft.Spatial.dll"
        };

        public override string Name => "OData 4";

        public override string Author => "Dmitrii Smirnov";

        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            return cxInfo.GetConnectionProperties().Uri;
        }

        public override Version Version => new Version("1.0.3.0");

        public override ParameterDescriptor[] GetContextConstructorParameters(IConnectionInfo cxInfo)
        {
            // We need to pass the chosen URI into the DataServiceContext's constructor:
            return new[] { new ParameterDescriptor("serviceRoot", "System.Uri") };
        }

        public override object[] GetContextConstructorArguments(IConnectionInfo cxInfo)
        {
            // We need to pass the chosen URI into the DataServiceContext's constructor:
            return new object[] { new Uri(cxInfo.GetConnectionProperties().Uri) };
        }

        public override IEnumerable<string> GetAssembliesToAdd(IConnectionInfo cxInfo)
        {
            // We need the following assembly for compiliation and autocompletion:
            return Assemblies;
        }

        public override IDbConnection GetIDbConnection(IConnectionInfo cxInfo)
        {
            return null;
        }

        public override IEnumerable<string> GetNamespacesToAdd(IConnectionInfo cxInfo)
        {
            return new[] { "Microsoft.OData.Client" };
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
        {
            // Populate the default URI with a demo value:
            if (isNewConnection)
            {
                cxInfo.GetConnectionProperties().Uri = "http://services.odata.org/V4/OData/OData.svc/";
            }

            return new ConnectionDialog(cxInfo).ShowDialog() == true;
        }

        public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo cxInfo, AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
        {
            var properties = cxInfo.GetConnectionProperties();

            // using code from Microsoft's OData v4 Client Code Generator. see https://visualstudiogallery.msdn.microsoft.com/9b786c0e-79d1-4a50-89a5-125e57475937
            var client = new ODataClient(new Configuration(properties.Uri, nameSpace, properties.GetCredentials(), properties.GetWebProxy()));
            var code = client.GenerateCode();

            // Compile the code into the assembly, using the assembly name provided:
            var assembly = BuildAssembly(code, assemblyToBuild);

            typeName = GetContainerName(properties);
            var containerName = string.Concat(nameSpace, ".", typeName);
            var containerType = assembly.GetType(containerName);
            // Use the schema to populate the Schema Explorer:
            var schema = GetSchema(containerType, assembly);

            return schema;
        }

        private Assembly BuildAssembly(string code, AssemblyName assemblyToBuild)
        {
            // Use the CSharpCodeProvider to compile the generated code:
            CompilerResults results;
            using (var codeProvider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } }))
            {
                // transform path to assemblies
                var assemblies = Assemblies.Select(o => Path.Combine(GetDriverFolder(), o)).Union(SystemAssemblies).ToArray();
                var options = new CompilerParameters(assemblies, assemblyToBuild.CodeBase, true);
                results = codeProvider.CompileAssemblyFromSource(options, code);
            }

            if (results.Errors.Count > 0)
            {
                throw new Exception($"Cannot compile typed context: {results.Errors[0].ErrorText} (line {results.Errors[0].Line})");
            }

            return results.CompiledAssembly;
        }

        /// <summary> Get main schema container name for given service uri </summary>
        /// <param name="properties">connection info</param>
        /// <returns>Container name</returns>
        static string GetContainerName(ConnectionProperties properties)
        {
            // odata v4 client inherites from DataServiceContext, use it for get IEdmModel
            IEdmModel model;
            
            var settings = new XmlReaderSettings
            {
                XmlResolver = new XmlUrlResolver
                {
                    Credentials = properties.GetCredentials(),
                    Proxy = properties.GetWebProxy()
                }
            };

            using (var reader = XmlReader.Create(properties.Uri + "/$metadata", settings))
            {
                model = EdmxReader.Parse(reader);
            }

            var root = model.SchemaElements.OfType<IEdmEntityContainer>().Single();

            // FIX ?
            var containerName = model.DeclaredNamespaces.Count() > 1 ? root.FullName() : root.Name;

            return containerName;
        }

        // TODO: parse service IEdmModel instead of generated assembly
        private static List<ExplorerItem> GetSchema(Type customType, Assembly assembly)
        {
            var list = customType.GetProperties()
                .Where(o => o.DeclaringType == customType &&
                            typeof(IQueryable).IsAssignableFrom(o.PropertyType))
                .OrderBy(o => o.Name)
                .Select(o => new ExplorerItem(o.Name, ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
                {
                    IsEnumerable = true,
                    ToolTipText = o.PropertyType.Name,

                    // Store the entity type to the Tag property. We'll use it later.
                    Tag = o.PropertyType.GetGenericArguments()[0]
                })
                .ToList();

            // Create a lookup keying each element type to the properties of that type. This will allow
            // us to build hyperlink targets allowing the user to click between associations:
            var elementTypeLookup = list.ToLookup(tp => (Type)tp.Tag);

            // Populate the columns (properties) of each entity:
            foreach (var table in list)
            {
                var parentType = (Type)table.Tag;
                table.Children = parentType.GetProperties()
                     .Select(childProp => GetChildItem(elementTypeLookup, childProp, parentType, assembly))
                     .OrderBy(childItem => childItem.Kind)
                     .ToList();
            }

            return list;
        }

        // TODO: simplificate ugly code
        private static ExplorerItem GetChildItem(ILookup<Type, ExplorerItem> elementTypeLookup, PropertyInfo childProp, Type parentType, Assembly assembly)
        {
            ExplorerIcon icon;

            // If the property's type is in our list of entities, then it's a Many:1 (or 1:1) reference.
            // We'll assume it's a Many:1 (we can't reliably identify 1:1s purely from reflection).
            if (elementTypeLookup.Contains(childProp.PropertyType))
            {
                icon = ExplorerIcon.ManyToOne;

                if (childProp.PropertyType.GetProperties().Any(o => o.PropertyType == parentType))
                {
                    icon = ExplorerIcon.OneToOne;
                }

                return new ExplorerItem(childProp.Name, ExplorerItemKind.ReferenceLink, icon)
                {
                    HyperlinkTarget = elementTypeLookup[childProp.PropertyType].First(),
                    // FormatTypeName is a helper method that returns a nicely formatted type name.
                    ToolTipText = childProp.PropertyType.Name
                };
            }

            // Is the property's type a collection of entities?
            var ienumerableOfT = childProp.PropertyType.GetInterface("System.Collections.Generic.IEnumerable`1");
            if (ienumerableOfT != null)
            {
                var elementType = ienumerableOfT.GetGenericArguments().First();
                if (elementTypeLookup.Contains(elementType))
                {
                    icon = ExplorerIcon.OneToMany;

                    if (elementType.GetProperties().Any(o => o.PropertyType.IsGenericType && o.PropertyType.GetGenericArguments().First() == parentType))
                    {
                        icon = ExplorerIcon.ManyToMany;
                    }

                    return new ExplorerItem(childProp.Name, ExplorerItemKind.CollectionLink, icon)
                    {
                        HyperlinkTarget = elementTypeLookup[elementType].First(),
                        ToolTipText = elementType.Name
                    };
                }
            }

            // Ordinary property:
            icon = ExplorerIcon.Column;
            var keyAttribute = childProp.DeclaringType.GetCustomAttribute<KeyAttribute>();
            if (keyAttribute != null && keyAttribute.KeyNames.Contains(childProp.Name))
            {
                icon = ExplorerIcon.Key;
            }

            if (childProp.PropertyType.Assembly == assembly)
            {
                // type in our dynamic data context assembly
                return new ExplorerItem(childProp.Name, ExplorerItemKind.ReferenceLink, ExplorerIcon.OneToOne);
            }

            // system data type (or not listed in model?)
            var propertyTypeStr = GetFullTypeName(childProp.PropertyType);

            return new ExplorerItem(childProp.Name + " (" + (propertyTypeStr) + ")", ExplorerItemKind.Property, icon);
        }

        private static string GetFullTypeName(Type t)
        {
            if (!t.IsGenericType)
            {
                return t.Name;
            }

            if (t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return t.GetGenericArguments().Single() + "?";
            }

            var sb = new StringBuilder();

            sb.Append(t.Name.Substring(0, t.Name.LastIndexOf("`", StringComparison.Ordinal)));
            var i = 0;
            t.GetGenericArguments().Aggregate(sb, (a, type) => a.Append(i++ == 0 ? "<" : ",").Append(GetFullTypeName(type)));
            sb.Append(">");

            return sb.ToString();
        }

        public override void InitializeContext(IConnectionInfo cxInfo, object context, QueryExecutionManager executionManager)
        {
            var dsContext = (DataServiceContext)context;

            var properties = cxInfo.GetConnectionProperties();
            dsContext.Credentials = properties.GetCredentials();

            dsContext.Configurations.RequestPipeline.OnMessageCreating += args =>
            {
                var message = new CustomizedRequestMessage(args, properties.GetWebProxy());
                return message;
            };
            
            dsContext.SendingRequest2 += (s, e) => executionManager.SqlTranslationWriter.WriteLine(e.RequestMessage.Url);
        }

        public override bool AreRepositoriesEquivalent(IConnectionInfo r1, IConnectionInfo r2)
        {
            return Equals(r1.DriverData.Element("Uri"), r2.DriverData.Element("Uri"));
        }
    }
}