using System;
using LINQPad.Extensibility.DataContext;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using OData4.LINQPadDriver.Templates;

namespace OData4.LINQPadDriver
{
	public class DynamicDriver : DynamicDataContextDriver
	{
		private const string BaseNamespacePrefix = "LINQPad.User";

		/// <summary> Assemblies, using both to compile generated code and to load into LINQPad </summary>
		private static readonly string[] Assemblies =
		{
			"Microsoft.OData.Client.dll",
			"Microsoft.OData.Core.dll",
			"Microsoft.OData.Edm.dll",
			"Microsoft.Spatial.dll",
			"Azure.Identity.dll",
			"Azure.Core.dll",
			"System.Windows.Interactivity.dll",
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
			// We need the following assembly for compilation and auto-completion:
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
				_namespaces.Add("Azure.Identity");
				_namespaces.Add("Azure.Core");
			}

			return _namespaces;
		}

		public override bool ShowConnectionDialog(IConnectionInfo connectionInfo, ConnectionDialogOptions dialogOptions)
		{
			var connectionProperties = connectionInfo.GetConnectionProperties();

			// Populate the default URI with a demo data:
			if (dialogOptions.IsNewConnection)
				connectionProperties.Uri = "https://services.odata.org/TripPinRESTierService";

			return new ConnectionDialog(connectionProperties).ShowDialog() == true;
		}

		public override void PreprocessObjectToWrite(ref object objectToWrite, ObjectGraphInfo info)
		{
			if (objectToWrite is DataServiceQuery dataServiceQuery)
			{
				objectToWrite = dataServiceQuery.ExecuteAsync().GetAwaiter().GetResult();
			}
		}

		// ReSharper disable once RedundantAssignment
		public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo connectionInfo, AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
		{
			var properties = connectionInfo.GetConnectionProperties();

			var codeGenerator = new ODataT4CodeGenerator
			{
				MetadataDocumentUri = properties.Uri,
				NamespacePrefix = nameSpace,
				TargetLanguage = ODataT4CodeGenerator.LanguageOption.CSharp,
				UseDataServiceCollection = false,
				EnableNamingAlias = false,
				IgnoreUnexpectedElementsAndAttributes = true,
				Properties = properties,
			};
			var code = codeGenerator.TransformText();

			BuildAssembly(code, assemblyToBuild);

			var model = properties.GetModel();

			typeName = GetContainerName(model);
			var schema = model.GetSchema();

			return schema;
		}

		private static void BuildAssembly(string code, AssemblyName assemblyToBuild)
		{
			var assemblies = new List<string>
			{
				typeof(IEdmModel).Assembly.Location,
				typeof(DataServiceQuery).Assembly.Location,
				typeof(Microsoft.Spatial.Geometry).Assembly.Location,
			};

			var references = GetCoreFxReferenceAssemblies()
				.Concat(assemblies)
				.Select(x => MetadataReference.CreateFromFile(x));

			var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

			var compilation = CSharpCompilation
				.Create(assemblyToBuild.FullName)
				.WithOptions(options)
				.AddReferences(references)
				.AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));

			using var fileStream = File.OpenWrite(assemblyToBuild.CodeBase);

			var results = compilation.Emit(fileStream);

			if (results.Success)
			{
				return;
			}

			var msg = results
				.Diagnostics
				.Aggregate("Can't compile typed context:", (s, e) => s + Environment.NewLine + e.GetMessage());

			throw new Exception(msg);
		}

		/// <summary> Get main schema container name for given service uri </summary>
		/// <param name="model">Entity Data Model</param>
		/// <returns>Container name</returns>
		private static string GetContainerName(IEdmModel model)
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

			var writer = executionManager.SqlTranslationWriter;

			if (writer == null) // we in lprun and haven't log writer
				return;

			dsContext.SendingRequest2 += (s, e) =>
			{
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
