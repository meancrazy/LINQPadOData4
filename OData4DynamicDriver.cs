using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using LINQPad.Extensibility.DataContext;
using Microsoft.CSharp;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using OData4.Builder;
using OData4.UI;

namespace OData4
{
    public class OData4DynamicDriver : DynamicDataContextDriver
    {
        private const string BaseNamespacePrefix = "LINQPad.User";

        /// <summary> System assemblies, using to compile generated odata client code </summary>
        private static readonly string[] SystemAssemblies =
        {
            "System.dll",
            "System.Core.dll",
            "System.Runtime.dll",
            "System.Xml.dll",
            "System.Xml.ReaderWriter.dll",
        };

        /// <summary> Assemblies, using both to compile generated code and to load into LINQPad </summary>
        private static readonly string[] Assemblies = {
            "Microsoft.OData.Client.dll",
            "Microsoft.OData.Core.dll",
            "Microsoft.OData.Edm.dll",
            "Microsoft.Spatial.dll",
        };

        private List<string> _namespaces;

        public override string Name => "OData v4 Connection";

        public override string Author => "Dmitrii Smirnov";

        public override string GetConnectionDescription(IConnectionInfo connectionInfo)
        {
            return connectionInfo.GetConnectionProperties().Uri;
        }

        public override ParameterDescriptor[] GetContextConstructorParameters(IConnectionInfo connectionInfo)
        {
            // We need to pass the chosen URI into the DataServiceContext's constructor:
            return new[] { new ParameterDescriptor("serviceRoot", "System.Uri") };
        }

        public override object[] GetContextConstructorArguments(IConnectionInfo connectionInfo)
        {
            // We need to pass the chosen URI into the DataServiceContext's constructor:
            return new object[] { new Uri(connectionInfo.GetConnectionProperties().Uri) };
        }

        public override IEnumerable<string> GetAssembliesToAdd(IConnectionInfo connectionInfo)
        {
            // We need the following assembly for compiliation and autocompletion:
            return Assemblies;
        }

        public override IDbConnection GetIDbConnection(IConnectionInfo connectionInfo)
        {
            return null;
        }

        public override IEnumerable<string> GetNamespacesToAdd(IConnectionInfo connectionInfo)
        {
            if (_namespaces == null || _namespaces.Count == 0)
            {
                var model = connectionInfo.GetConnectionProperties().GetModel();
                var namespaces = model.DeclaredNamespaces.Select(o => BaseNamespacePrefix + "." + o).ToList();
                _namespaces = namespaces.Count > 1 ? namespaces.ToList() : new List<string>(1);
                _namespaces.Add("Microsoft.OData.Client");
            }

            return _namespaces;
        }

        public override bool ShowConnectionDialog(IConnectionInfo connectionInfo, bool isNewConnection)
        {
            var connectionProperties = connectionInfo.GetConnectionProperties();

            // Populate the default URI with a demo data:
            if (isNewConnection)
                connectionProperties.Uri = "http://services.odata.org/V4/OData/OData.svc/";

            return new ConnectionDialog(connectionProperties).ShowDialog() == true;
        }

        public override void PreprocessObjectToWrite(ref object objectToWrite, ObjectGraphInfo info)
        {
            if (objectToWrite is DataServiceQuery)
                objectToWrite = ((DataServiceQuery)objectToWrite).Execute();

            /*if (objectToWrite.GetType().GetGenericTypeDefinition() == typeof(DataServiceQuerySingle<>))
            {
                var methodInfo = objectToWrite.GetType().GetMethod("GetValue");
                methodInfo.Invoke(objectToWrite, null);
            }*/
        }

        public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo connectionInfo, AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
        {
            var properties = connectionInfo.GetConnectionProperties();

            // using code from Microsoft's OData v4 Client Code Generator. see https://visualstudiogallery.msdn.microsoft.com/9b786c0e-79d1-4a50-89a5-125e57475937
            var client = new ODataClient(new Configuration(properties.Uri, nameSpace, properties));
            var code = client.GenerateCode();

            // Compile the code into the assembly, using the assembly name provided:
            BuildAssembly(code, assemblyToBuild);

            var model = properties.GetModel();

            typeName = GetContainerName(model);
            var schema = model.GetSchema();

            return schema;
        }

        private void BuildAssembly(string code, AssemblyName assemblyToBuild)
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

            if (results.Errors.Count <= 0)
                return;

            var msg = results.Errors.Cast<CompilerError>()
                             .Aggregate("Can't compile typed context:", (s, e) => s + Environment.NewLine + e.ErrorText);

            throw new Exception(msg);
        }
        
        /// <summary> Get main schema container name for given service uri </summary>
        /// <param name="model">Entity Data Model</param>
        /// <returns>Container name</returns>
        static string GetContainerName(IEdmModel model)
        {
            var root = model.EntityContainer;

            // FIX ?
            var containerName = model.DeclaredNamespaces.Count() > 1 ? root.FullName() : root.Name;

            return containerName;
        }

        public override void InitializeContext(IConnectionInfo connectionInfo, object context, QueryExecutionManager executionManager)
        {
            var dsContext = (DataServiceContext)context;

            var properties = connectionInfo.GetConnectionProperties();
            dsContext.Credentials = properties.GetCredentials();

            dsContext.Configurations.RequestPipeline.OnMessageCreating += args =>
            {
                var message = new CustomizedRequestMessage(args, properties);
                return message;
            };
            
            dsContext.SendingRequest2 += (s, e) =>
            {
                var writer = executionManager.SqlTranslationWriter;

                if (writer == null) // we in lprun and haven't log writer
                    return;

                writer.WriteLine($"URL:\t\t{e.RequestMessage.Url}");
                
                if (properties.LogMethod)
                    writer.WriteLine($"Method:\t{e.RequestMessage.Method}");

                if (properties.LogHeaders)
                {
                    writer.WriteLine("Headers:");
                    var headers = string.Join("\r\n", e.RequestMessage.Headers.Select(o => $"\t{o.Key}:{o.Value}"));
                    writer.WriteLine(headers);
                }
            };
        }

        public override bool AreRepositoriesEquivalent(IConnectionInfo r1, IConnectionInfo r2)
        {
            return Equals(r1.DriverData.Element("Uri"), r2.DriverData.Element("Uri"));
        }
    }
}