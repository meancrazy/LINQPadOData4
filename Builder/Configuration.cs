using System.Net;

namespace OData4.Builder
{
    public class Configuration
    {
        // The URI of the metadata document. The value must be set to a valid service document URI or a local file path 
        // eg : "http://services.odata.org/V4/OData/OData.svc/", "File:///C:/Odata.edmx", or @"C:\Odata.edmx"
        // ### Notice ### If the OData service requires authentication for accessing the metadata document, the value of
        // MetadataDocumentUri has to be set to a local file path, or the client code generation process will fail.
        public string MetadataDocumentUri { get; }

        // The use of DataServiceCollection enables entity and property tracking. The value must be set to true or false.
        public bool UseDataServiceCollection => false;

        // The namespace of the client code generated. It replaces the original namespace in the metadata document, 
        // unless the model has several namespaces.
        public string NamespacePrefix { get; }

        // This flag indicates whether to enable naming alias. The value must be set to true or false.
        public bool EnableNamingAlias => false;

        // This flag indicates whether to ignore unexpected elements and attributes in the metadata document and generate
        // the client code if any. The value must be set to true or false.
        public bool IgnoreUnexpectedElementsAndAttributes => true;

        public ICredentials Credentials { get; }

        public IWebProxy WebProxy { get; }

        public bool AcceptInvalidCertificate { get; }

        public Configuration(string uri, string namespacePrefix, ICredentials credentials, IWebProxy webProxy, bool acceptInvalidCertificate)
        {
            MetadataDocumentUri = uri;
            NamespacePrefix = namespacePrefix;
            Credentials = credentials;
            WebProxy = webProxy;
            AcceptInvalidCertificate = acceptInvalidCertificate;
        }
    }
}