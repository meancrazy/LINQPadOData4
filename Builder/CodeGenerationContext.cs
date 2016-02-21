using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Annotations;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Expressions;
using Microsoft.OData.Edm.Library;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Vocabularies.Community.V1;
using Microsoft.OData.Edm.Vocabularies.V1;

namespace OData4.Builder
{
    /// <summary>
    /// Context object to provide the model and configuration info to the code generator.
    /// </summary>
    public class CodeGenerationContext
    {
        /// <summary>
        /// The namespace of the term to use when building value annotations for indicating the conventions used.
        /// </summary>
        private const string ConventionTermNamespace = "Com.Microsoft.OData.Service.Conventions.V1";

        /// <summary>
        /// The name of the term to use when building value annotations for indicating the conventions used.
        /// </summary>
        private const string ConventionTermName = "UrlConventions";

        /// <summary>
        /// The string value for indicating that the key-as-segment convention is being used in annotations and headers.
        /// </summary>
        private const string KeyAsSegmentConventionName = "KeyAsSegment";

        /// <summary>
        /// The namespacePrefix is used as the only namespace in generated code when there's only one schema in edm model,
        /// and as a prefix for the namespace from the model with multiple schemas. If this argument is null, the
        /// namespaces from the model are used for all types.
        /// </summary>
        private readonly string _namespacePrefix;

        /// <summary>
        /// The EdmModel to generate code for.
        /// </summary>
        private IEdmModel _edmModel;

        /// <summary>
        /// The array of namespaces in the current edm model.
        /// </summary>
        private string[] _namespacesInModel;

        /// <summary>
        /// The array of warnings occured when parsing edm model.
        /// </summary>
        private string[] _warnings;

        /// <summary>
        /// true if the model contains any structural type with inheritance, false otherwise.
        /// </summary>
        private bool? _modelHasInheritance;

        /// <summary>
        /// If the namespacePrefix is not null, this contains the mapping of namespaces in the model to the corresponding prefixed namespaces.
        /// Otherwise this is an empty dictionary.
        /// </summary>
        private Dictionary<string, string> _namespaceMap;

        /// <summary>
        /// Maps the element type of a navigation source to the navigation source.
        /// </summary>
        private Dictionary<IEdmEntityType, List<IEdmNavigationSource>> _elementTypeToNavigationSourceMap;

        /// <summary>
        /// HashSet contains the pair of Names and Namespaces of EntityContainers using KeyAsSegment url convention
        /// </summary>
        private HashSet<string> _keyAsSegmentContainers;

        /// <summary>
        /// Constructs an instance of <see cref="CodeGenerationContext"/>.
        /// </summary>
        /// <param name="metadataUri">The Uri to the metadata document. The supported scheme are File, http and https.</param>
        /// <param name="namespacePrefix">The namespacePrefix is used as the only namespace in generated code
        /// when there's only one schema in edm model, and as a prefix for the namespace from the model with multiple
        /// schemas. If this argument is null, the namespaces from the model are used for all types.</param>
        /// <param name="credentials"></param>
        /// <param name="webProxy"></param>
        public CodeGenerationContext(Uri metadataUri, string namespacePrefix, ICredentials credentials, IWebProxy webProxy) : this(GetEdmxStringFromMetadataPath(metadataUri, credentials, webProxy), namespacePrefix, credentials, webProxy)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="CodeGenerationContext"/>.
        /// </summary>
        /// <param name="edmx">The string for the edmx.</param>
        /// <param name="namespacePrefix">The namespacePrefix is used as the only namespace in generated code
        /// when there's only one schema in edm model, and as a prefix for the namespace from the model with multiple
        /// schemas. If this argument is null, the namespaces from the model are used for all types.</param>
        /// <param name="credentials"></param>
        /// <param name="webProxy"></param>
        private CodeGenerationContext(string edmx, string namespacePrefix, ICredentials credentials, IWebProxy webProxy)
        {
            Edmx = XElement.Parse(edmx);
            _namespacePrefix = namespacePrefix;
            _credentials = credentials;
            _webProxy = webProxy;
        }

        /// <summary> The EdmModel to generate code for. </summary>
        public XElement Edmx { get; }

        /// <summary>
        /// The EdmModel to generate code for.
        /// </summary>
        public IEdmModel EdmModel
        {
            get
            {
                if (_edmModel == null)
                {
                    Debug.Assert(Edmx != null, "this.edmx != null");

                    IEnumerable<EdmError> errors;
                    var edmxReaderSettings = new EdmxReaderSettings
                    {
                        GetReferencedModelReaderFunc = uri => GetReferencedModelReaderFuncWrapper(uri, _credentials, _webProxy),
                        IgnoreUnexpectedAttributesAndElements = IgnoreUnexpectedElementsAndAttributes
                    };

                    if (!EdmxReader.TryParse(Edmx.CreateReader(ReaderOptions.None), Enumerable.Empty<IEdmModel>(), edmxReaderSettings, out _edmModel, out errors))
                    {
                        Debug.Assert(errors != null, "errors != null");
                        throw new InvalidOperationException(errors.FirstOrDefault().ErrorMessage);
                    }

                    if (IgnoreUnexpectedElementsAndAttributes)
                    {
                        if (errors != null && errors.Any())
                        {
                            _warnings = errors.Select(e => e.ErrorMessage).ToArray();
                        }
                    }
                }

                return _edmModel;
            }
        }

        /// <summary> The func for user code to overwrite and provide referenced model's XmlReader. </summary>
        private readonly Func<Uri, ICredentials, IWebProxy, XmlReader> _getReferencedModelReaderFunc = (uri, credentials, webProxy) => XmlReader.Create(GetEdmxStreamFromUri(uri, credentials, webProxy), Settings);

        /// <summary>
        /// Basic setting for XmlReader.
        /// </summary>
        private static readonly XmlReaderSettings Settings = new XmlReaderSettings { IgnoreWhitespace = true };

        /// <summary>
        /// The Wrapper func for user code to overwrite and provide referenced model's stream.
        /// </summary>
        public Func<Uri, ICredentials, IWebProxy, XmlReader> GetReferencedModelReaderFuncWrapper
        {
            get
            {
                return (uri, credentials, webProxy) =>
                {
                    using (var reader = _getReferencedModelReaderFunc(uri, credentials, webProxy))
                    {
                        if (reader == null)
                        {
                            return null;
                        }

                        var element = XElement.Load(reader);
                        if (ReferencesMap == null)
                        {
                            ReferencesMap = new Dictionary<Uri, XElement>();
                        }

                        ReferencesMap.Add(uri, element);
                        return element.CreateReader(ReaderOptions.None);
                    }
                };
            }
        }

        /// <summary>
        /// Dictionary that stores uri and referenced xml mapping.
        /// </summary>
        public Dictionary<Uri, XElement> ReferencesMap { get; set; }

        /// <summary>
        /// The array of namespaces in the current edm model.
        /// </summary>
        public string[] NamespacesInModel
        {
            get
            {
                if (_namespacesInModel == null)
                {
                    Debug.Assert(EdmModel != null, "this.EdmModel != null");
                    _namespacesInModel = GetElementsFromModelTree(EdmModel, m => m.SchemaElements.Select(e => e.Namespace)).Distinct().ToArray();
                }

                return _namespacesInModel;
            }
        }

        /// <summary>
        /// The array of warnings occured when parsing edm model.
        /// </summary>
        public string[] Warnings => _warnings ?? (_warnings = new string[] { });

        /// <summary>
        /// true if the model contains any structural type with inheritance, false otherwise.
        /// </summary>
        public bool ModelHasInheritance
        {
            get
            {
                if (!_modelHasInheritance.HasValue)
                {
                    Debug.Assert(EdmModel != null, "this.EdmModel != null");
                    _modelHasInheritance = EdmModel.SchemaElementsAcrossModels().OfType<IEdmStructuredType>().Any(t => t.BaseType != null);
                }

                return _modelHasInheritance.Value;
            }
        }

        /// <summary>
        /// true if we need to generate the ResolveNameFromType method, false otherwise.
        /// </summary>
        public bool NeedResolveNameFromType => ModelHasInheritance || NamespaceMap.Count > 0 || EnableNamingAlias;

        /// <summary>
        /// true if we need to generate the ResolveTypeFromName method, false otherwise.
        /// </summary>
        public bool NeedResolveTypeFromName => NamespaceMap.Count > 0 || EnableNamingAlias;

        /// <summary>
        /// If the namespacePrefix is not null, this contains the mapping of namespaces in the model to the corresponding prefixed namespaces.
        /// Otherwise this is an empty dictionary.
        /// </summary>
        public Dictionary<string, string> NamespaceMap
        {
            get
            {
                if (_namespaceMap == null)
                {
                    if (!string.IsNullOrEmpty(_namespacePrefix))
                    {
                        if (NamespacesInModel.Count() == 1)
                        {
                            var container = EdmModel.EntityContainer;
                            var containerNamespace = container == null ? null : container.Namespace;
                            _namespaceMap = NamespacesInModel
                                .Distinct()
                                .ToDictionary(
                                    ns => ns,
                                    ns => ns == containerNamespace ?
                                        _namespacePrefix :
                                        _namespacePrefix + "." + (EnableNamingAlias ? Customization.CustomizeNamespace(ns) : ns));
                        }
                        else
                        {
                            _namespaceMap = NamespacesInModel
                                .Distinct()
                                .ToDictionary(
                                    ns => ns,
                                    ns => _namespacePrefix + "." + (EnableNamingAlias ? Customization.CustomizeNamespace(ns) : ns));
                        }
                    }
                    else if (EnableNamingAlias)
                    {
                        _namespaceMap = NamespacesInModel.Distinct()
                                                         .ToDictionary(ns => ns, Customization.CustomizeNamespace);
                    }
                    else
                    {
                        _namespaceMap = new Dictionary<string, string>();
                    }
                }

                return _namespaceMap;
            }
        }

        /// <summary>
        /// true to use DataServiceCollection in the generated code, false otherwise.
        /// </summary>
        public bool UseDataServiceCollection { get; set; }

        /// <summary>
        /// true to use Upper camel case for all class and property names, false otherwise.
        /// </summary>
        public bool EnableNamingAlias { get; set; }

        /// <summary>
        /// true to ignore unknown elements or attributes in metadata, false otherwise.
        /// </summary>
        public bool IgnoreUnexpectedElementsAndAttributes { private get; set; }

        /// <summary>
        /// Maps the element type of an entity set to the entity set.
        /// </summary>
        public Dictionary<IEdmEntityType, List<IEdmNavigationSource>> ElementTypeToNavigationSourceMap => _elementTypeToNavigationSourceMap ?? (_elementTypeToNavigationSourceMap = new Dictionary<IEdmEntityType, List<IEdmNavigationSource>>(EqualityComparer<IEdmEntityType>.Default));
        
        private readonly ICredentials _credentials;
        
        private readonly IWebProxy _webProxy;

        /// <summary>
        /// true if this EntityContainer need to set the UrlConvention to KeyAsSegment, false otherwise.
        /// </summary>
        public bool UseKeyAsSegmentUrlConvention(IEdmEntityContainer currentContainer)
        {
            if (_keyAsSegmentContainers == null)
            {
                _keyAsSegmentContainers = new HashSet<string>();
                Debug.Assert(EdmModel != null, "this.EdmModel != null");
                var annotations = EdmModel.VocabularyAnnotations;
                foreach (var valueAnnotation in annotations.OfType<IEdmValueAnnotation>())
                {
                    var container = valueAnnotation.Target as IEdmEntityContainer;
                    var valueTerm = valueAnnotation.Term as IEdmValueTerm;
                    var expression = valueAnnotation.Value as IEdmStringConstantExpression;
                    if (container != null && valueTerm != null && expression != null)
                    {
                        if (valueTerm.Namespace == ConventionTermNamespace &&
                            valueTerm.Name == ConventionTermName &&
                            expression.Value == KeyAsSegmentConventionName)
                        {
                            _keyAsSegmentContainers.Add(container.FullName());
                        }
                    }
                }
            }

            return _keyAsSegmentContainers.Contains(currentContainer.FullName());
        }

        /// <summary>
        /// Gets the enumeration of schema elements with the given namespace.
        /// </summary>
        /// <param name="ns">The namespace of the schema elements to get.</param>
        /// <returns>The enumeration of schema elements with the given namespace.</returns>
        public IEnumerable<IEdmSchemaElement> GetSchemaElements(string ns)
        {
            Debug.Assert(ns != null, "ns != null");
            Debug.Assert(EdmModel != null, "this.EdmModel != null");
            return GetElementsFromModelTree(EdmModel, m => m.SchemaElements.Where(e => e.Namespace == ns));
        }

        /// <summary>
        /// Gets the namespace qualified name for the given <paramref name="schemaElement"/> with the namespace prefix applied if this.NamespacePrefix is specified.
        /// </summary>
        /// <param name="schemaElement">The schema element to get the full name for.</param>
        /// <param name="schemaElementFixedName">The fixed name of this schemaElement.</param>
        /// <param name="template">The current code generate template.</param>
        /// <param name="needGlobalPrefix"></param>
        /// <returns>The namespace qualified name for the given <paramref name="schemaElement"/> with the namespace prefix applied if this.NamespacePrefix is specified.</returns>
        public string GetPrefixedFullName(IEdmSchemaElement schemaElement, string schemaElementFixedName, ODataClientTemplate template, bool needGlobalPrefix = true)
        {
            if (schemaElement == null)
            {
                return null;
            }

            return GetPrefixedNamespace(schemaElement.Namespace, template, true, needGlobalPrefix) + "." + schemaElementFixedName;
        }

        /// <summary>
        /// Gets the prefixed namespace for the given <paramref name="ns"/>.
        /// </summary>
        /// <param name="ns">The namespace without the prefix.</param>
        /// <param name="template">The current code generate template.</param>
        /// <param name="needFix">The flag indicates whether the namespace need to be fixed now.</param>
        /// <param name="needGlobalPrefix">The flag indicates whether the namespace need to be added by gloabal prefix.</param>
        /// <returns>The prefixed namespace for the given <paramref name="ns"/>.</returns>
        public string GetPrefixedNamespace(string ns, ODataClientTemplate template, bool needFix, bool needGlobalPrefix)
        {
            if (ns == null)
            {
                return null;
            }

            string prefixedNamespace;
            if (!NamespaceMap.TryGetValue(ns, out prefixedNamespace))
            {
                prefixedNamespace = ns;
            }

            if (needFix)
            {
                var segments = prefixedNamespace.Split('.');
                prefixedNamespace = string.Empty;
                var n = segments.Length;
                for (var i = 0; i < n; ++i)
                {
                    if (template.LanguageKeywords.Contains(segments[i]))
                    {
                        prefixedNamespace += string.Format(template.FixPattern, segments[i]);
                    }
                    else
                    {
                        prefixedNamespace += segments[i];
                    }

                    prefixedNamespace += (i == n - 1 ? string.Empty : ".");
                }
            }

            if (needGlobalPrefix)
            {
                prefixedNamespace = template.GlobalPrefix + prefixedNamespace;
            }

            return prefixedNamespace;
        }

        /// <summary>
        /// Reads the edmx string from a file path or a http/https path.
        /// </summary>
        /// <param name="metadataUri">The Uri to the metadata document. The supported scheme are File, http and https.</param>
        /// <param name="credentials"></param>
        /// <param name="webProxy"></param>
        private static string GetEdmxStringFromMetadataPath(Uri metadataUri, ICredentials credentials, IWebProxy webProxy)
        {
            string content;
            using (var streamReader = new StreamReader(GetEdmxStreamFromUri(metadataUri, credentials, webProxy)))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
        }

        /// <summary>
        /// Get the metadata stream from a file path or a http/https path.
        /// </summary>
        /// <param name="metadataUri">The Uri to the stream. The supported scheme are File, http and https.</param>
        /// <param name="credentials"></param>
        /// <param name="webProxy"></param>
        private static Stream GetEdmxStreamFromUri(Uri metadataUri, ICredentials credentials, IWebProxy webProxy)
        {
            Debug.Assert(metadataUri != null, "metadataUri != null");
            Stream metadataStream;
            if (metadataUri.Scheme == "file")
            {
                metadataStream = new FileStream(Uri.UnescapeDataString(metadataUri.AbsolutePath), FileMode.Open, FileAccess.Read);
            }
            else if (metadataUri.Scheme == "http" || metadataUri.Scheme == "https")
            {
                try
                {
                    var webRequest = (HttpWebRequest)WebRequest.Create(metadataUri);
                    webRequest.Credentials = credentials;
                    webRequest.Proxy = webProxy;

                    var webResponse = webRequest.GetResponse();
                    metadataStream = webResponse.GetResponseStream();
                }
                catch (WebException e)
                {
                    var webResponse = e.Response as HttpWebResponse;
                    if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new WebException("Failed to access the metadata document. The OData service requires authentication for accessing it. Please download the metadata, store it into a local file, and set the value of “MetadataDocumentUri” in the .odata.config file to the file path. After that, run custom tool again to generate the OData Client code.");
                    }
                    throw;
                }
            }
            else
            {
                throw new ArgumentException("Only file, http, https schemes are supported for paths to metadata source locations.");
            }

            return metadataStream;
        }

        private static IEnumerable<T> GetElementsFromModelTree<T>(IEdmModel mainModel, Func<IEdmModel, IEnumerable<T>> getElementFromOneModelFunc)
        {
            var ret = new List<T>();
            if (mainModel is EdmCoreModel || mainModel.FindDeclaredValueTerm(CoreVocabularyConstants.OptimisticConcurrencyControl) != null)
            {
                return ret;
            }

            ret.AddRange(getElementFromOneModelFunc(mainModel));
            foreach (var tmp in mainModel.ReferencedModels)
            {
                if (tmp is EdmCoreModel ||
                    tmp.FindDeclaredValueTerm(CoreVocabularyConstants.OptimisticConcurrencyControl) != null ||
                    tmp.FindDeclaredValueTerm(CapabilitiesVocabularyConstants.ChangeTracking) != null ||
                    tmp.FindDeclaredValueTerm(AlternateKeysVocabularyConstants.AlternateKeys) != null)
                {
                    continue;
                }

                ret.AddRange(getElementFromOneModelFunc(tmp));
            }

            return ret;
        }
    }
}