using System;
using System.Collections.Generic;
using System.Globalization;

namespace OData4.Builder
{
    public sealed class ODataClientCSharpTemplate : ODataClientTemplate
    {
        /// <summary>
        /// Creates an instance of the ODataClientTemplate.
        /// </summary>
        /// <param name="context">The code generation context.</param>
        public ODataClientCSharpTemplate(CodeGenerationContext context)
            : base(context)
        {
        }

        internal override string GlobalPrefix => "global::";
        internal override string SystemTypeTypeName => "global::System.Type";
        internal override string AbstractModifier => " abstract";
        internal override string DataServiceActionQueryTypeName => "global::Microsoft.OData.Client.DataServiceActionQuery";
        internal override string DataServiceActionQuerySingleOfTStructureTemplate => "global::Microsoft.OData.Client.DataServiceActionQuerySingle<{0}>";
        internal override string DataServiceActionQueryOfTStructureTemplate => "global::Microsoft.OData.Client.DataServiceActionQuery<{0}>";
        internal override string NotifyPropertyChangedModifier => "global::System.ComponentModel.INotifyPropertyChanged";
        internal override string ClassInheritMarker => " : ";
        internal override string ParameterSeparator => ", \r\n                    ";
        internal override string KeyParameterSeparator => ", \r\n            ";
        internal override string KeyDictionaryItemSeparator => ", \r\n                ";
        internal override string SystemNullableStructureTemplate => "global::System.Nullable<{0}>";
        internal override string CollectionOfTStructureTemplate => "global::System.Collections.Generic.ICollection<{0}>";
        internal override string DataServiceCollectionStructureTemplate => "global::Microsoft.OData.Client.DataServiceCollection<{0}>";
        internal override string DataServiceQueryStructureTemplate => "global::Microsoft.OData.Client.DataServiceQuery<{0}>";
        internal override string DataServiceQuerySingleStructureTemplate => "global::Microsoft.OData.Client.DataServiceQuerySingle<{0}>";
        internal override string ObservableCollectionStructureTemplate => "global::System.Collections.ObjectModel.ObservableCollection<{0}>";
        internal override string ObjectModelCollectionStructureTemplate => "global::System.Collections.ObjectModel.Collection<{0}>";
        internal override string DataServiceCollectionConstructorParameters => "(null, global::Microsoft.OData.Client.TrackingMode.None)";
        internal override string NewModifier => "new ";
        internal override string GeoTypeInitializePattern => "global::Microsoft.Spatial.SpatialImplementation.CurrentImplementation.CreateWellKnownTextSqlFormatter(false).Read<{0}>(new global::System.IO.StringReader(\"{1}\"))";
        internal override string Int32TypeName => "int";
        internal override string StringTypeName => "string";
        internal override string BinaryTypeName => "byte[]";
        internal override string DecimalTypeName => "decimal";
        internal override string Int16TypeName => "short";
        internal override string SingleTypeName => "float";
        internal override string BooleanTypeName => "bool";
        internal override string DoubleTypeName => "double";
        internal override string GuidTypeName => "global::System.Guid";
        internal override string ByteTypeName => "byte";
        internal override string Int64TypeName => "long";
        internal override string SByteTypeName => "sbyte";
        internal override string DataServiceStreamLinkTypeName => "global::Microsoft.OData.Client.DataServiceStreamLink";
        internal override string GeographyTypeName => "global::Microsoft.Spatial.Geography";
        internal override string GeographyPointTypeName => "global::Microsoft.Spatial.GeographyPoint";
        internal override string GeographyLineStringTypeName => "global::Microsoft.Spatial.GeographyLineString";
        internal override string GeographyPolygonTypeName => "global::Microsoft.Spatial.GeographyPolygon";
        internal override string GeographyCollectionTypeName => "global::Microsoft.Spatial.GeographyCollection";
        internal override string GeographyMultiPolygonTypeName => "global::Microsoft.Spatial.GeographyMultiPolygon";
        internal override string GeographyMultiLineStringTypeName => "global::Microsoft.Spatial.GeographyMultiLineString";
        internal override string GeographyMultiPointTypeName => "global::Microsoft.Spatial.GeographyMultiPoint";
        internal override string GeometryTypeName => "global::Microsoft.Spatial.Geometry";
        internal override string GeometryPointTypeName => "global::Microsoft.Spatial.GeometryPoint";
        internal override string GeometryLineStringTypeName => "global::Microsoft.Spatial.GeometryLineString";
        internal override string GeometryPolygonTypeName => "global::Microsoft.Spatial.GeometryPolygon";
        internal override string GeometryCollectionTypeName => "global::Microsoft.Spatial.GeometryCollection";
        internal override string GeometryMultiPolygonTypeName => "global::Microsoft.Spatial.GeometryMultiPolygon";
        internal override string GeometryMultiLineStringTypeName => "global::Microsoft.Spatial.GeometryMultiLineString";
        internal override string GeometryMultiPointTypeName => "global::Microsoft.Spatial.GeometryMultiPoint";
        internal override string DateTypeName => "global::Microsoft.OData.Edm.Library.Date";
        internal override string DateTimeOffsetTypeName => "global::System.DateTimeOffset";
        internal override string DurationTypeName => "global::System.TimeSpan";
        internal override string TimeOfDayTypeName => "global::Microsoft.OData.Edm.Library.TimeOfDay";
        internal override string XmlConvertClassName => "global::System.Xml.XmlConvert";
        internal override string EnumTypeName => "global::System.Enum";
        internal override string FixPattern => "@{0}";
        internal override string EnumUnderlyingTypeMarker => " : ";
        internal override string ConstantExpressionConstructorWithType => "global::System.Linq.Expressions.Expression.Constant({0}, typeof({1}))";
        internal override string TypeofFormatter => "typeof({0})";
        internal override string UriOperationParameterConstructor => "new global::Microsoft.OData.Client.UriOperationParameter(\"{0}\", {1})";
        internal override string UriEntityOperationParameterConstructor => "new global::Microsoft.OData.Client.UriEntityOperationParameter(\"{0}\", {1}, {2})";
        internal override string BodyOperationParameterConstructor => "new global::Microsoft.OData.Client.BodyOperationParameter(\"{0}\", {1})";
        internal override string BaseEntityType => " : global::Microsoft.OData.Client.BaseEntityType";
        internal override string OverloadsModifier => "new ";
        internal override string ODataVersion => "global::Microsoft.OData.Core.ODataVersion.V4";
        internal override string ParameterDeclarationTemplate => "{0} {1}";
        internal override string DictionaryItemConstructor => "{{ {0}, {1} }}";

        internal override HashSet<string> LanguageKeywords => _cSharpKeywords ?? (_cSharpKeywords = new HashSet<string>(StringComparer.Ordinal)
        {
            "abstract",
            "as",
            "base",
            "byte",
            "bool",
            "break",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "for",
            "foreach",
            "finally",
            "fixed",
            "float",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "string",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "volatile",
            "void",
            "while"
        });

        private HashSet<string> _cSharpKeywords;

        internal override void WriteFileHeader()
        {

            Write("//------------------------------------------------------------------------------\r" +
                  "\n// <auto-generated>\r\n//     This code was generated by a tool.\r\n//     Runtime " +
                  "Version:");

            Write(ToStringHelper.ToStringWithCulture(Environment.Version));

            Write("\r\n//\r\n//     Changes to this file may cause incorrect behavior and will be lost i" +
                  "f\r\n//     the code is regenerated.\r\n// </auto-generated>\r\n//--------------------" +
                  "----------------------------------------------------------\r\n\r\n// Generation date" +
                  ": ");

            Write(ToStringHelper.ToStringWithCulture(DateTime.Now.ToString(CultureInfo.CurrentCulture)));

            Write("\r\n");


        }

        internal override void WriteNamespaceStart(string fullNamespace)
        {

            Write("namespace ");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write("\r\n{\r\n");


        }

        internal override void WriteClassStartForEntityContainer(string originalContainerName, string containerName, string fixedContainerName)
        {

            Write("    /// <summary>\r\n    /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(containerName));

            Write(" in the schema.\r\n    /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("    [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalContainerName));

                Write("\")]\r\n");


            }

            Write("    public partial class ");

            Write(ToStringHelper.ToStringWithCulture(fixedContainerName));

            Write(" : global::Microsoft.OData.Client.DataServiceContext\r\n    {\r\n");
        }

        internal override void WriteMethodStartForEntityContainerConstructor(string containerName, string fixedContainerName)
        {

            Write("        /// <summary>\r\n        /// Initialize a new ");

            Write(ToStringHelper.ToStringWithCulture(containerName));

            Write(" object.\r\n        /// </summary>\r\n        [global::System.CodeDom.Compiler.Genera" +
                  "tedCodeAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        public ");

            Write(ToStringHelper.ToStringWithCulture(fixedContainerName));

            Write("(global::System.Uri serviceRoot) : \r\n                base(serviceRoot, global::Mi" +
                  "crosoft.OData.Client.ODataProtocolVersion.V4)\r\n        {\r\n");
        }

        internal override void WriteKeyAsSegmentUrlConvention()
        {

            Write("            this.UrlConventions = global::Microsoft.OData.Client.DataServiceUrlCo" +
                  "nventions.KeyAsSegment;\r\n");


        }

        internal override void WriteInitializeResolveName()
        {

            Write("            this.ResolveName = new global::System.Func<global::System.Type, strin" +
                  "g>(this.ResolveNameFromType);\r\n");


        }

        internal override void WriteInitializeResolveType()
        {

            Write("            this.ResolveType = new global::System.Func<string, global::System.Typ" +
                  "e>(this.ResolveTypeFromName);\r\n");


        }

        internal override void WriteClassEndForEntityContainerConstructor()
        {

            Write("            this.OnContextCreated();\r\n            this.Format.LoadServiceModel = " +
                  "GeneratedEdmModel.GetInstance;\r\n            this.Format.UseJson();\r\n        }\r\n " +
                  "       partial void OnContextCreated();\r\n");


        }

        internal override void WriteMethodStartForResolveTypeFromName()
        {

            Write(@"        /// <summary>
        /// Since the namespace configured for this service reference
        /// in Visual Studio is different from the one indicated in the
        /// server schema, use type-mappers to map between the two.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""Microsoft.OData.Client.Design.T4"", """);

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        protected global::System.Type ResolveTypeFromName(string typeName)\r\n" +
                  "        {\r\n");


        }

        internal override void WriteResolveNamespace(string typeName, string fullNamespace, string languageDependentNamespace)
        {

            Write("            ");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write("resolvedType = this.DefaultResolveType(typeName, \"");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write("\", \"");

            Write(ToStringHelper.ToStringWithCulture(languageDependentNamespace));

            Write("\");\r\n            if ((resolvedType != null))\r\n            {\r\n                retu" +
                  "rn resolvedType;\r\n            }\r\n");


        }

        internal override void WriteMethodEndForResolveTypeFromName()
        {

            Write("            return null;\r\n        }\r\n");


        }

        internal override void WritePropertyRootNamespace(string containerName, string fullNamespace)
        {

        }

        internal override void WriteMethodStartForResolveNameFromType(string containerName, string fullNamespace)
        {

            Write(@"        /// <summary>
        /// Since the namespace configured for this service reference
        /// in Visual Studio is different from the one indicated in the
        /// server schema, use type-mappers to map between the two.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""Microsoft.OData.Client.Design.T4"", """);

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        protected string ResolveNameFromType(global::System.Type clientType)" +
                  "\r\n        {\r\n");


            if (Context.EnableNamingAlias)
            {

                Write(@"            global::Microsoft.OData.Client.OriginalNameAttribute originalNameAttribute = (global::Microsoft.OData.Client.OriginalNameAttribute)global::System.Linq.Enumerable.SingleOrDefault(global::Microsoft.OData.Client.Utility.GetCustomAttributes(clientType, typeof(global::Microsoft.OData.Client.OriginalNameAttribute), true));
");


            }
        }

        internal override void WriteResolveType(string fullNamespace, string languageDependentNamespace)
        {

            Write("            if (clientType.Namespace.Equals(\"");

            Write(ToStringHelper.ToStringWithCulture(languageDependentNamespace));

            Write("\", global::System.StringComparison.Ordinal))\r\n            {\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("                if (originalNameAttribute != null)\r\n                {\r\n          " +
                      "          return string.Concat(\"");

                Write(ToStringHelper.ToStringWithCulture(fullNamespace));

                Write(".\", originalNameAttribute.OriginalName);\r\n                }\r\n");


            }

            Write("                return string.Concat(\"");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write(".\", clientType.Name);\r\n            }\r\n");


        }

        internal override void WriteMethodEndForResolveNameFromType(bool modelHasInheritance)
        {
            if (Context.EnableNamingAlias && modelHasInheritance)
            {

                Write("            if (originalNameAttribute != null)\r\n            {\r\n                re" +
                      "turn clientType.Namespace + \".\" + originalNameAttribute.OriginalName;\r\n         " +
                      "   }\r\n");


            }

            Write("            return ");

            Write(ToStringHelper.ToStringWithCulture(modelHasInheritance ? "clientType.FullName" : "null"));

            Write(";\r\n        }\r\n");


        }

        internal override void WriteConstructorForSingleType(string singleTypeName, string baseTypeName)
        {

            Write("        /// <summary>\r\n        /// Initialize a new ");

            Write(ToStringHelper.ToStringWithCulture(singleTypeName));

            Write(" object.\r\n        /// </summary>\r\n        public ");

            Write(ToStringHelper.ToStringWithCulture(singleTypeName));

            Write("(global::Microsoft.OData.Client.DataServiceContext context, string path)\r\n       " +
                  "     : base(context, path) {}\r\n\r\n        /// <summary>\r\n        /// Initialize a" +
                  " new ");

            Write(ToStringHelper.ToStringWithCulture(singleTypeName));

            Write(" object.\r\n        /// </summary>\r\n        public ");

            Write(ToStringHelper.ToStringWithCulture(singleTypeName));

            Write("(global::Microsoft.OData.Client.DataServiceContext context, string path, bool isC" +
                  "omposable)\r\n            : base(context, path, isComposable) {}\r\n\r\n        /// <s" +
                  "ummary>\r\n        /// Initialize a new ");

            Write(ToStringHelper.ToStringWithCulture(singleTypeName));

            Write(" object.\r\n        /// </summary>\r\n        public ");

            Write(ToStringHelper.ToStringWithCulture(singleTypeName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(baseTypeName));

            Write(" query)\r\n            : base(query) {}\r\n\r\n");


        }

        internal override void WriteContextEntitySetProperty(string entitySetName, string entitySetFixedName, string originalEntitySetName, string entitySetElementTypeName, bool inContext = true)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write(" in the schema.\r\n        /// </summary>\r\n        [global::System.CodeDom.Compiler" +
                  ".GeneratedCodeAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalEntitySetName));

                Write("\")]\r\n");


            }

            Write("        public global::Microsoft.OData.Client.DataServiceQuery<");

            Write(ToStringHelper.ToStringWithCulture(entitySetElementTypeName));

            Write("> ");

            Write(ToStringHelper.ToStringWithCulture(entitySetFixedName));

            Write("\r\n        {\r\n            get\r\n            {\r\n");


            if (!inContext)
            {

                Write("                if (!this.IsComposable)\r\n                {\r\n                    t" +
                      "hrow new global::System.NotSupportedException(\"The previous function is not comp" +
                      "osable.\");\r\n                }\r\n");


            }

            Write("                if ((this._");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write(" == null))\r\n                {\r\n                    this._");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write(" = ");

            Write(ToStringHelper.ToStringWithCulture(inContext ? "base" : "Context"));

            Write(".CreateQuery<");

            Write(ToStringHelper.ToStringWithCulture(entitySetElementTypeName));

            Write(">(");

            Write(ToStringHelper.ToStringWithCulture(inContext ? "\"" + originalEntitySetName + "\"" : "GetPath(\"" + originalEntitySetName + "\")"));

            Write(");\r\n                }\r\n                return this._");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write(";\r\n            }\r\n        }\r\n        [global::System.CodeDom.Compiler.GeneratedCo" +
                  "deAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        private global::Microsoft.OData.Client.DataServiceQuery<");

            Write(ToStringHelper.ToStringWithCulture(entitySetElementTypeName));

            Write("> _");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write(";\r\n");


        }

        internal override void WriteContextSingletonProperty(string singletonName, string singletonFixedName, string originalSingletonName, string singletonElementTypeName, bool inContext = true)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(singletonName));

            Write(" in the schema.\r\n        /// </summary>\r\n        [global::System.CodeDom.Compiler" +
                  ".GeneratedCodeAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalSingletonName));

                Write("\")]\r\n");


            }

            Write("        public ");

            Write(ToStringHelper.ToStringWithCulture(singletonElementTypeName));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(singletonFixedName));

            Write("\r\n        {\r\n            get\r\n            {\r\n");


            if (!inContext)
            {

                Write("                if (!this.IsComposable)\r\n                {\r\n                    t" +
                      "hrow new global::System.NotSupportedException(\"The previous function is not comp" +
                      "osable.\");\r\n                }\r\n");


            }

            Write("                if ((this._");

            Write(ToStringHelper.ToStringWithCulture(singletonName));

            Write(" == null))\r\n                {\r\n                    this._");

            Write(ToStringHelper.ToStringWithCulture(singletonName));

            Write(" = new ");

            Write(ToStringHelper.ToStringWithCulture(singletonElementTypeName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(inContext ? "this" : "this.Context"));

            Write(", ");

            Write(ToStringHelper.ToStringWithCulture(inContext ? "\"" + originalSingletonName + "\"" : "GetPath(\"" + originalSingletonName + "\")"));

            Write(");\r\n                }\r\n                return this._");

            Write(ToStringHelper.ToStringWithCulture(singletonName));

            Write(";\r\n            }\r\n        }\r\n        [global::System.CodeDom.Compiler.GeneratedCo" +
                  "deAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        private ");

            Write(ToStringHelper.ToStringWithCulture(singletonElementTypeName));

            Write(" _");

            Write(ToStringHelper.ToStringWithCulture(singletonName));

            Write(";\r\n");


        }

        internal override void WriteContextAddToEntitySetMethod(string entitySetName, string originalEntitySetName, string typeName, string parameterName)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write(" in the schema.\r\n        /// </summary>\r\n        [global::System.CodeDom.Compiler" +
                  ".GeneratedCodeAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        public void AddTo");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(parameterName));

            Write(")\r\n        {\r\n            base.AddObject(\"");

            Write(ToStringHelper.ToStringWithCulture(originalEntitySetName));

            Write("\", ");

            Write(ToStringHelper.ToStringWithCulture(parameterName));

            Write(");\r\n        }\r\n");


        }

        internal override void WriteGeneratedEdmModel(string escapedEdmxString)
        {

            Write("        [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsoft.OData." +
                  "Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        private abstract class GeneratedEdmModel\r\n        {\r\n");


            if (Context.ReferencesMap != null)
            {

                Write("            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsoft.OD" +
                      "ata.Client.Design.T4\", \"");

                Write(ToStringHelper.ToStringWithCulture(T4Version));

                Write("\")]\r\n            private static global::System.Collections.Generic.Dictionary<str" +
                      "ing, string> ReferencesMap = new global::System.Collections.Generic.Dictionary<s" +
                      "tring, string>()\r\n                {\r\n");


                foreach (var reference in Context.ReferencesMap)
                {

                    Write("                    {@\"");

                    Write(ToStringHelper.ToStringWithCulture(reference.Key.OriginalString.Replace("\"", "\"\"")));

                    Write("\", @\"");

                    Write(ToStringHelper.ToStringWithCulture(Utils.SerializeToString(reference.Value).Replace("\"", "\"\"")));

                    Write("\"},\r\n");


                }

                Write("                };\r\n");


            }

            Write("            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsoft.OD" +
                  "ata.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n            private static global::Microsoft.OData.Edm.IEdmModel ParsedModel" +
                  " = LoadModelFromString();\r\n            [global::System.CodeDom.Compiler.Generate" +
                  "dCodeAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n            private const string Edmx = @\"");

            Write(ToStringHelper.ToStringWithCulture(escapedEdmxString));

            Write("\";\r\n            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsof" +
                  "t.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n            public static global::Microsoft.OData.Edm.IEdmModel GetInstance(" +
                  ")\r\n            {\r\n                return ParsedModel;\r\n            }\r\n");


            if (Context.ReferencesMap != null)
            {

                Write("            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsoft.OD" +
                      "ata.Client.Design.T4\", \"");

                Write(ToStringHelper.ToStringWithCulture(T4Version));

                Write(@""")]
            private static global::System.Xml.XmlReader getReferencedModelFromMap(global::System.Uri uri)
            {
                string referencedEdmx;
                if (ReferencesMap.TryGetValue(uri.OriginalString, out referencedEdmx))
                {
                    return CreateXmlReader(referencedEdmx);
                }

                return null;
            }
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""Microsoft.OData.Client.Design.T4"", """);

                Write(ToStringHelper.ToStringWithCulture(T4Version));

                Write(@""")]
            private static global::Microsoft.OData.Edm.IEdmModel LoadModelFromString()
            {
                global::System.Xml.XmlReader reader = CreateXmlReader(Edmx);
                try
                {
                    return global::Microsoft.OData.Edm.Csdl.CsdlReader.Parse(reader, getReferencedModelFromMap);
                }
                finally
                {
                    ((global::System.IDisposable)(reader)).Dispose();
                }
            }
");


            }
            else
            {

                Write("            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsoft.OD" +
                      "ata.Client.Design.T4\", \"");

                Write(ToStringHelper.ToStringWithCulture(T4Version));

                Write(@""")]
            private static global::Microsoft.OData.Edm.IEdmModel LoadModelFromString()
            {
                global::System.Xml.XmlReader reader = CreateXmlReader(Edmx);
                try
                {
                    return global::Microsoft.OData.Edm.Csdl.CsdlReader.Parse(reader);
                }
                finally
                {
                    ((global::System.IDisposable)(reader)).Dispose();
                }
            }
");


            }

            Write("            [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsoft.OD" +
                  "ata.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n            private static global::System.Xml.XmlReader CreateXmlReader(stri" +
                  "ng edmxToParse)\r\n            {\r\n                return global::System.Xml.XmlRea" +
                  "der.Create(new global::System.IO.StringReader(edmxToParse));\r\n            }\r\n   " +
                  "     }\r\n");


        }

        internal override void WriteClassEndForEntityContainer()
        {

            Write("    }\r\n");


        }

        internal override void WriteSummaryCommentForStructuredType(string typeName)
        {

            Write("    /// <summary>\r\n    /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write(" in the schema.\r\n    /// </summary>\r\n");


        }

        internal override void WriteKeyPropertiesCommentAndAttribute(IEnumerable<string> keyProperties, string keyString)
        {

            Write("    /// <KeyProperties>\r\n");


            foreach (var key in keyProperties)
            {

                Write("    /// ");

                Write(ToStringHelper.ToStringWithCulture(key));

                Write("\r\n");


            }

            Write("    /// </KeyProperties>\r\n    [global::Microsoft.OData.Client.Key(\"");

            Write(ToStringHelper.ToStringWithCulture(keyString));

            Write("\")]\r\n");


        }

        internal override void WriteEntityTypeAttribute()
        {

            Write("    [global::Microsoft.OData.Client.EntityType()]\r\n");


        }

        internal override void WriteEntitySetAttribute(string entitySetName)
        {

            Write("    [global::Microsoft.OData.Client.EntitySet(\"");

            Write(ToStringHelper.ToStringWithCulture(entitySetName));

            Write("\")]\r\n");


        }

        internal override void WriteEntityHasStreamAttribute()
        {

            Write("    [global::Microsoft.OData.Client.HasStream()]\r\n");


        }

        internal override void WriteClassStartForStructuredType(string abstractModifier, string typeName, string originalTypeName, string baseTypeName)
        {
            if (Context.EnableNamingAlias)
            {

                Write("    [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalTypeName));

                Write("\")]\r\n");


            }

            Write("    public");

            Write(ToStringHelper.ToStringWithCulture(abstractModifier));

            Write(" partial class ");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write(ToStringHelper.ToStringWithCulture(baseTypeName));

            Write("\r\n    {\r\n");


        }

        internal override void WriteSummaryCommentForStaticCreateMethod(string typeName)
        {

            Write("        /// <summary>\r\n        /// Create a new ");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write(" object.\r\n        /// </summary>\r\n");


        }

        internal override void WriteParameterCommentForStaticCreateMethod(string parameterName, string propertyName)
        {

            Write("        /// <param name=\"");

            Write(ToStringHelper.ToStringWithCulture(parameterName));

            Write("\">Initial value of ");

            Write(ToStringHelper.ToStringWithCulture(propertyName));

            Write(".</param>\r\n");


        }

        internal override void WriteDeclarationStartForStaticCreateMethod(string typeName, string fixedTypeName)
        {

            Write("        [global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Microsoft.OData." +
                  "Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        public static ");

            Write(ToStringHelper.ToStringWithCulture(fixedTypeName));

            Write(" Create");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write("(");


        }

        internal override void WriteParameterForStaticCreateMethod(string parameterTypeName, string parameterName, string parameterSeparater)
        {

            Write(ToStringHelper.ToStringWithCulture(parameterTypeName));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(parameterName));

            Write(ToStringHelper.ToStringWithCulture(parameterSeparater));


        }

        internal override void WriteDeclarationEndForStaticCreateMethod(string typeName, string instanceName)
        {

            Write(")\r\n        {\r\n            ");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(instanceName));

            Write(" = new ");

            Write(ToStringHelper.ToStringWithCulture(typeName));

            Write("();\r\n");


        }

        internal override void WriteParameterNullCheckForStaticCreateMethod(string parameterName)
        {

            Write("            if ((");

            Write(ToStringHelper.ToStringWithCulture(parameterName));

            Write(" == null))\r\n            {\r\n                throw new global::System.ArgumentNullE" +
                  "xception(\"");

            Write(ToStringHelper.ToStringWithCulture(parameterName));

            Write("\");\r\n            }\r\n");


        }

        internal override void WritePropertyValueAssignmentForStaticCreateMethod(string instanceName, string propertyName, string parameterName)
        {

            Write("            ");

            Write(ToStringHelper.ToStringWithCulture(instanceName));

            Write(".");

            Write(ToStringHelper.ToStringWithCulture(propertyName));

            Write(" = ");

            Write(ToStringHelper.ToStringWithCulture(parameterName));

            Write(";\r\n");


        }

        internal override void WriteMethodEndForStaticCreateMethod(string instanceName)
        {

            Write("            return ");

            Write(ToStringHelper.ToStringWithCulture(instanceName));

            Write(";\r\n        }\r\n");


        }

        internal override void WritePropertyForStructuredType(string propertyType, string originalPropertyName, string propertyName, string fixedPropertyName, string privatePropertyName, string propertyInitializationValue, bool writeOnPropertyChanged)
        {

            Write("        /// <summary>\r\n        /// There are no comments for Property ");

            Write(ToStringHelper.ToStringWithCulture(propertyName));

            Write(" in the schema.\r\n        /// </summary>\r\n        [global::System.CodeDom.Compiler" +
                  ".GeneratedCodeAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n");


            if (Context.EnableNamingAlias || IdentifierMappings.ContainsKey(originalPropertyName))
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalPropertyName));

                Write("\")]\r\n");


            }

            Write("        public ");

            Write(ToStringHelper.ToStringWithCulture(propertyType));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(fixedPropertyName));

            Write("\r\n        {\r\n            get\r\n            {\r\n                return this.");

            Write(ToStringHelper.ToStringWithCulture(privatePropertyName));

            Write(";\r\n            }\r\n            set\r\n            {\r\n                this.On");

            Write(ToStringHelper.ToStringWithCulture(propertyName));

            Write("Changing(value);\r\n                this.");

            Write(ToStringHelper.ToStringWithCulture(privatePropertyName));

            Write(" = value;\r\n                this.On");

            Write(ToStringHelper.ToStringWithCulture(propertyName));

            Write("Changed();\r\n");


            if (writeOnPropertyChanged)
            {

                Write("                this.OnPropertyChanged(\"");

                Write(ToStringHelper.ToStringWithCulture(originalPropertyName));

                Write("\");\r\n");


            }

            Write("            }\r\n        }\r\n        [global::System.CodeDom.Compiler.GeneratedCodeA" +
                  "ttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write("\")]\r\n        private ");

            Write(ToStringHelper.ToStringWithCulture(propertyType));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(privatePropertyName));

            Write(ToStringHelper.ToStringWithCulture(propertyInitializationValue != null ? " = " + propertyInitializationValue : string.Empty));

            Write(";\r\n        partial void On");

            Write(ToStringHelper.ToStringWithCulture(propertyName));

            Write("Changing(");

            Write(ToStringHelper.ToStringWithCulture(propertyType));

            Write(" value);\r\n        partial void On");

            Write(ToStringHelper.ToStringWithCulture(propertyName));

            Write("Changed();\r\n");


        }

        internal override void WriteINotifyPropertyChangedImplementation()
        {

            Write("        /// <summary>\r\n        /// This event is raised when the value of the pro" +
                  "perty is changed\r\n        /// </summary>\r\n        [global::System.CodeDom.Compil" +
                  "er.GeneratedCodeAttribute(\"Microsoft.OData.Client.Design.T4\", \"");

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write(@""")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// The value of the property is changed
        /// </summary>
        /// <param name=""property"">property name</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""Microsoft.OData.Client.Design.T4"", """);

            Write(ToStringHelper.ToStringWithCulture(T4Version));

            Write(@""")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
");


        }

        internal override void WriteClassEndForStructuredType()
        {

            Write("    }\r\n");


        }

        internal override void WriteEnumFlags()
        {

            Write("    [global::System.Flags]\r\n");


        }

        internal override void WriteSummaryCommentForEnumType(string enumName)
        {

            Write("    /// <summary>\r\n    /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(enumName));

            Write(" in the schema.\r\n    /// </summary>\r\n");


        }

        internal override void WriteEnumDeclaration(string enumName, string originalEnumName, string underlyingType)
        {
            if (Context.EnableNamingAlias)
            {

                Write("    [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalEnumName));

                Write("\")]\r\n");


            }

            Write("    public enum ");

            Write(ToStringHelper.ToStringWithCulture(enumName));

            Write(ToStringHelper.ToStringWithCulture(underlyingType));

            Write("\r\n    {\r\n");


        }

        internal override void WriteMemberForEnumType(string member, string originalMemberName, bool last)
        {
            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalMemberName));

                Write("\")]\r\n");


            }

            Write("        ");

            Write(ToStringHelper.ToStringWithCulture(member));

            Write(ToStringHelper.ToStringWithCulture(last ? string.Empty : ","));

            Write("\r\n");


        }

        internal override void WriteEnumEnd()
        {

            Write("    }\r\n");


        }

        internal override void WriteFunctionImportReturnCollectionResult(string functionName, string originalFunctionName, string returnTypeName, string parameters, string parameterValues, bool isComposable, bool useEntityReference)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

                Write("\")]\r\n");


            }

            Write("        public global::Microsoft.OData.Client.DataServiceQuery<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("> ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(parameters));

            Write(ToStringHelper.ToStringWithCulture(useEntityReference ? ", bool useEntityReference = false" : string.Empty));

            Write(")\r\n        {\r\n            return this.CreateFunctionQuery<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(">(\"\", \"");

            Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

            Write("\", ");

            Write(ToStringHelper.ToStringWithCulture(isComposable.ToString().ToLower()));

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(");\r\n        }\r\n");


        }

        internal override void WriteFunctionImportReturnSingleResult(string functionName, string originalFunctionName, string returnTypeName, string parameters, string parameterValues, bool isComposable, bool isReturnEntity, bool useEntityReference)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

                Write("\")]\r\n");


            }

            Write("        public ");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? returnTypeName + SingleSuffix : string.Format(DataServiceQuerySingleStructureTemplate, returnTypeName)));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(parameters));

            Write(ToStringHelper.ToStringWithCulture(useEntityReference ? ", bool useEntityReference = false" : string.Empty));

            Write(")\r\n        {\r\n            return ");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? "new " + returnTypeName + SingleSuffix + "(" : string.Empty));

            Write("this.CreateFunctionQuerySingle<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(">(\"\", \"");

            Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

            Write("\", ");

            Write(ToStringHelper.ToStringWithCulture(isComposable.ToString().ToLower()));

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(")");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? ")" : string.Empty));

            Write(";\r\n        }\r\n");


        }

        internal override void WriteBoundFunctionInEntityTypeReturnCollectionResult(bool hideBaseMethod, string functionName, string originalFunctionName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool useEntityReference)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

                Write("\")]\r\n");


            }

            Write("        public ");

            Write(ToStringHelper.ToStringWithCulture(hideBaseMethod ? OverloadsModifier : string.Empty));

            Write("global::Microsoft.OData.Client.DataServiceQuery<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("> ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(parameters));

            Write(ToStringHelper.ToStringWithCulture(useEntityReference ? ", bool useEntityReference = false" : string.Empty));

            Write(")\r\n        {\r\n            global::System.Uri requestUri;\r\n            Context.Try" +
                  "GetUri(this, out requestUri);\r\n            return this.Context.CreateFunctionQue" +
                  "ry<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(">(string.Join(\"/\", global::System.Linq.Enumerable.Select(global::System.Linq.Enum" +
                  "erable.Skip(requestUri.Segments, this.Context.BaseUri.Segments.Length), s => s.T" +
                  "rim(\'/\'))), \"");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write(".");

            Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

            Write("\", ");

            Write(ToStringHelper.ToStringWithCulture(isComposable.ToString().ToLower()));

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(ToStringHelper.ToStringWithCulture(useEntityReference ? ", bool useEntityReference = false" : string.Empty));

            Write(");\r\n        }\r\n");


        }

        internal override void WriteBoundFunctionInEntityTypeReturnSingleResult(bool hideBaseMethod, string functionName, string originalFunctionName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool isReturnEntity, bool useEntityReference)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

                Write("\")]\r\n");


            }

            Write("        public ");

            Write(ToStringHelper.ToStringWithCulture(hideBaseMethod ? OverloadsModifier : string.Empty));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? returnTypeName + SingleSuffix : string.Format(DataServiceQuerySingleStructureTemplate, returnTypeName)));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(parameters));

            Write(ToStringHelper.ToStringWithCulture(useEntityReference ? ", bool useEntityReference = false" : string.Empty));

            Write(")\r\n        {\r\n            global::System.Uri requestUri;\r\n            Context.Try" +
                  "GetUri(this, out requestUri);\r\n\r\n            return ");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? "new " + returnTypeName + SingleSuffix + "(" : string.Empty));

            Write("this.Context.CreateFunctionQuerySingle<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(">(string.Join(\"/\", global::System.Linq.Enumerable.Select(global::System.Linq.Enum" +
                  "erable.Skip(requestUri.Segments, this.Context.BaseUri.Segments.Length), s => s.T" +
                  "rim(\'/\'))), \"");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write(".");

            Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

            Write("\", ");

            Write(ToStringHelper.ToStringWithCulture(isComposable.ToString().ToLower()));

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(")");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? ")" : string.Empty));

            Write(";\r\n        }\r\n");


        }

        internal override void WriteActionImport(string actionName, string originalActionName, string returnTypeName, string parameters, string parameterValues)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(actionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalActionName));

                Write("\")]\r\n");


            }

            Write("        public ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(actionName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(parameters));

            Write(")\r\n        {\r\n            return new ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("(this, this.BaseUri.OriginalString.Trim(\'/\') + \"/");

            Write(ToStringHelper.ToStringWithCulture(originalActionName));

            Write("\"");

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(");\r\n        }\r\n");


        }

        internal override void WriteBoundActionInEntityType(bool hideBaseMethod, string actionName, string originalActionName, string returnTypeName, string parameters, string fullNamespace, string parameterValues)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(actionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalActionName));

                Write("\")]\r\n");


            }

            Write("        public ");

            Write(ToStringHelper.ToStringWithCulture(hideBaseMethod ? OverloadsModifier : string.Empty));

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(actionName));

            Write("(");

            Write(ToStringHelper.ToStringWithCulture(parameters));

            Write(@")
        {
            global::Microsoft.OData.Client.EntityDescriptor resource = Context.EntityTracker.TryGetEntityDescriptor(this);
            if (resource == null)
            {
                throw new global::System.Exception(""cannot find entity"");
            }

            return new ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("(this.Context, resource.EditLink.OriginalString.Trim(\'/\') + \"/");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write(".");

            Write(ToStringHelper.ToStringWithCulture(originalActionName));

            Write("\"");

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(");\r\n        }\r\n");


        }

        internal override void WriteExtensionMethodsStart()
        {

            Write("    /// <summary>\r\n    /// Class containing all extension methods\r\n    /// </summ" +
                  "ary>\r\n    public static class ExtensionMethods\r\n    {\r\n");


        }

        internal override void WriteExtensionMethodsEnd()
        {

            Write("    }\r\n");


        }

        internal override void WriteByKeyMethods(string entityTypeName, string returnTypeName, IEnumerable<string> keys, string keyParameters, string keyDictionaryItems)
        {

            Write("        /// <summary>\r\n        /// Get an entity of type ");

            Write(ToStringHelper.ToStringWithCulture(entityTypeName));

            Write(" as ");

            Write(ToStringHelper.ToStringWithCulture(entityTypeName + SingleSuffix));

            Write(" specified by key from an entity set\r\n        /// </summary>\r\n        /// <param " +
                  "name=\"source\">source entity set</param>\r\n        /// <param name=\"keys\">dictiona" +
                  "ry with the names and values of keys</param>\r\n        public static ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(" ByKey(this global::Microsoft.OData.Client.DataServiceQuery<");

            Write(ToStringHelper.ToStringWithCulture(entityTypeName));

            Write("> source, global::System.Collections.Generic.Dictionary<string, object> keys)\r\n  " +
                  "      {\r\n            return new ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetK" +
                  "eyString(source.Context, keys)));\r\n        }\r\n        /// <summary>\r\n        ///" +
                  " Get an entity of type ");

            Write(ToStringHelper.ToStringWithCulture(entityTypeName));

            Write(" as ");

            Write(ToStringHelper.ToStringWithCulture(entityTypeName + SingleSuffix));

            Write(" specified by key from an entity set\r\n        /// </summary>\r\n        /// <param " +
                  "name=\"source\">source entity set</param>\r\n");


            foreach (var key in keys)
            {

                Write("        /// <param name=\"");

                Write(ToStringHelper.ToStringWithCulture(key));

                Write("\">The value of ");

                Write(ToStringHelper.ToStringWithCulture(key));

                Write("</param>\r\n");


            }

            Write("        public static ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(" ByKey(this global::Microsoft.OData.Client.DataServiceQuery<");

            Write(ToStringHelper.ToStringWithCulture(entityTypeName));

            Write("> source,\r\n            ");

            Write(ToStringHelper.ToStringWithCulture(keyParameters));

            Write(")\r\n        {\r\n            global::System.Collections.Generic.Dictionary<string, o" +
                  "bject> keys = new global::System.Collections.Generic.Dictionary<string, object>\r" +
                  "\n            {\r\n                ");

            Write(ToStringHelper.ToStringWithCulture(keyDictionaryItems));

            Write("\r\n            };\r\n            return new ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("(source.Context, source.GetKeyPath(global::Microsoft.OData.Client.Serializer.GetK" +
                  "eyString(source.Context, keys)));\r\n        }\r\n");


        }

        internal override void WriteCastToMethods(string baseTypeName, string derivedTypeName, string derivedTypeFullName, string returnTypeName)
        {

            Write("        /// <summary>\r\n        /// Cast an entity of type ");

            Write(ToStringHelper.ToStringWithCulture(baseTypeName));

            Write(" to its derived type ");

            Write(ToStringHelper.ToStringWithCulture(derivedTypeFullName));

            Write("\r\n        /// </summary>\r\n        /// <param name=\"source\">source entity</param>\r" +
                  "\n        public static ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(" CastTo");

            Write(ToStringHelper.ToStringWithCulture(derivedTypeName));

            Write("(this global::Microsoft.OData.Client.DataServiceQuerySingle<");

            Write(ToStringHelper.ToStringWithCulture(baseTypeName));

            Write("> source)\r\n        {\r\n            global::Microsoft.OData.Client.DataServiceQuery" +
                  "Single<");

            Write(ToStringHelper.ToStringWithCulture(derivedTypeFullName));

            Write("> query = source.CastTo<");

            Write(ToStringHelper.ToStringWithCulture(derivedTypeFullName));

            Write(">();\r\n            return new ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("(source.Context, query.GetPath(null));\r\n        }\r\n");


        }

        internal override void WriteBoundFunctionReturnSingleResultAsExtension(string functionName, string originalFunctionName, string boundTypeName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool isReturnEntity, bool useEntityReference)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

                Write("\")]\r\n");


            }

            Write("        public static ");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? returnTypeName + SingleSuffix : string.Format(DataServiceQuerySingleStructureTemplate, returnTypeName)));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write("(this ");

            Write(ToStringHelper.ToStringWithCulture(boundTypeName));

            Write(" source");

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameters) ? string.Empty : ", " + parameters));

            Write(ToStringHelper.ToStringWithCulture(useEntityReference ? ", bool useEntityReference = false" : string.Empty));

            Write(")\r\n        {\r\n            if (!source.IsComposable)\r\n            {\r\n             " +
                  "   throw new global::System.NotSupportedException(\"The previous function is not " +
                  "composable.\");\r\n            }\r\n\r\n            return ");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? "new " + returnTypeName + SingleSuffix + "(" : string.Empty));

            Write("source.CreateFunctionQuerySingle<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(">(\"");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write(".");

            Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

            Write("\", ");

            Write(ToStringHelper.ToStringWithCulture(isComposable.ToString().ToLower()));

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(")");

            Write(ToStringHelper.ToStringWithCulture(isReturnEntity ? ")" : string.Empty));

            Write(";\r\n        }\r\n");


        }

        internal override void WriteBoundFunctionReturnCollectionResultAsExtension(string functionName, string originalFunctionName, string boundTypeName, string returnTypeName, string parameters, string fullNamespace, string parameterValues, bool isComposable, bool useEntityReference)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

                Write("\")]\r\n");


            }

            Write("        public static global::Microsoft.OData.Client.DataServiceQuery<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("> ");

            Write(ToStringHelper.ToStringWithCulture(functionName));

            Write("(this ");

            Write(ToStringHelper.ToStringWithCulture(boundTypeName));

            Write(" source");

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameters) ? string.Empty : ", " + parameters));

            Write(ToStringHelper.ToStringWithCulture(useEntityReference ? ", bool useEntityReference = true" : string.Empty));

            Write(")\r\n        {\r\n            if (!source.IsComposable)\r\n            {\r\n             " +
                  "   throw new global::System.NotSupportedException(\"The previous function is not " +
                  "composable.\");\r\n            }\r\n\r\n            return source.CreateFunctionQuery<");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(">(\"");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write(".");

            Write(ToStringHelper.ToStringWithCulture(originalFunctionName));

            Write("\", ");

            Write(ToStringHelper.ToStringWithCulture(isComposable.ToString().ToLower()));

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(");\r\n        }\r\n");


        }

        internal override void WriteBoundActionAsExtension(string actionName, string originalActionName, string boundSourceType, string returnTypeName, string parameters, string fullNamespace, string parameterValues)
        {

            Write("        /// <summary>\r\n        /// There are no comments for ");

            Write(ToStringHelper.ToStringWithCulture(actionName));

            Write(" in the schema.\r\n        /// </summary>\r\n");


            if (Context.EnableNamingAlias)
            {

                Write("        [global::Microsoft.OData.Client.OriginalNameAttribute(\"");

                Write(ToStringHelper.ToStringWithCulture(originalActionName));

                Write("\")]\r\n");


            }

            Write("        public static ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write(" ");

            Write(ToStringHelper.ToStringWithCulture(actionName));

            Write("(this ");

            Write(ToStringHelper.ToStringWithCulture(boundSourceType));

            Write(" source");

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameters) ? string.Empty : ", " + parameters));

            Write(")\r\n        {\r\n            if (!source.IsComposable)\r\n            {\r\n             " +
                  "   throw new global::System.NotSupportedException(\"The previous function is not " +
                  "composable.\");\r\n            }\r\n\r\n            return new ");

            Write(ToStringHelper.ToStringWithCulture(returnTypeName));

            Write("(source.Context, source.AppendRequestUri(\"");

            Write(ToStringHelper.ToStringWithCulture(fullNamespace));

            Write(".");

            Write(ToStringHelper.ToStringWithCulture(originalActionName));

            Write("\")");

            Write(ToStringHelper.ToStringWithCulture(string.IsNullOrEmpty(parameterValues) ? string.Empty : ", " + parameterValues));

            Write(");\r\n        }\r\n");


        }

        internal override void WriteNamespaceEnd()
        {

            Write("}\r\n");


        }
    }
}