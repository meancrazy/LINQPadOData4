using System;

namespace OData4.Builder
{
    /*
    OData Client T4 Template ver. 2.4.0
    Copyright (c) Microsoft Corporation
    All rights reserved. 
    MIT License
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    */
    internal class ODataClient
    {
        private readonly Configuration _configuration;

        public ODataClient(Configuration configuration)
        {
            _configuration = configuration;
            MetadataDocumentUri = configuration.MetadataDocumentUri;
            NamespacePrefix = configuration.NamespacePrefix;
        }

        public string GenerateCode()
        {
            var context = new CodeGenerationContext(new Uri(MetadataDocumentUri, UriKind.Absolute), NamespacePrefix, _configuration.Properties)
            {
                UseDataServiceCollection = _configuration.UseDataServiceCollection,
                EnableNamingAlias = _configuration.EnableNamingAlias,
                IgnoreUnexpectedElementsAndAttributes = _configuration.IgnoreUnexpectedElementsAndAttributes
            };

            var template = new ODataClientCSharpTemplate(context);
            return template.TransformText();
        }

        /// <summary>
        /// The Uri string to the metadata document.
        /// </summary>
        public string MetadataDocumentUri
        {
            get
            {
                return _metadataDocumentUri;
            }

            set
            {
                value = Uri.UnescapeDataString(value);
                Uri uri;
                if (!Uri.TryCreate(value, UriKind.Absolute, out uri))
                {
                    // ********************************************************************************************************
                    // To fix this error, if the current text transformation is run by the TextTemplatingFileGenerator
                    // custom tool inside Visual Studio, update the .odata.config file in the project with a valid parameter
                    // value then hit Ctrl-S to save the .tt file to refresh the code generation.
                    // ********************************************************************************************************
                    throw new ArgumentException($"The value \"{value}\" is not a valid MetadataDocumentUri because is it not a valid absolute Uri. The MetadataDocumentUri must be set to an absolute Uri referencing the $metadata endpoint of an OData service.");
                }

                if (uri.Scheme == "http" || uri.Scheme == "https")
                {
                    value = uri.Scheme + "://" + uri.Authority + uri.AbsolutePath;
                    value = value.TrimEnd('/');
                    if (!value.EndsWith("$metadata"))
                    {
                        value += "/$metadata";
                    }
                }

                _metadataDocumentUri = value;
            }
        }

        private string _metadataDocumentUri;

        /// <summary>
        /// The NamespacePrefix is used as the only namespace for types in the same namespace as the default container,
        /// and as a prefix for the namespace from the model for everything else. If this argument is null, the
        /// namespaces from the model are used for all types.
        /// </summary>
        public string NamespacePrefix
        {
            get
            {
                return _namespacePrefix;
            }

            set
            {
                _namespacePrefix = string.IsNullOrWhiteSpace(value) ? null : value;
            }
        }

        private string _namespacePrefix;
    }
}
