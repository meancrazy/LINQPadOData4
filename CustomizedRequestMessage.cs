using Microsoft.OData.Client;
using OData4.UI;

namespace OData4
{
    public class CustomizedRequestMessage : HttpWebRequestMessage
    {
        public CustomizedRequestMessage(DataServiceClientRequestMessageArgs args, ConnectionProperties properties) : base(args)
        {
            HttpWebRequest.Proxy = properties.GetWebProxy();
            HttpWebRequest.Headers.Add(properties.GetCustomHeaders());

            var cert = properties.GetClientCertificate();
            if (cert != null)
            {
                HttpWebRequest.ClientCertificates.Add(cert);
            }
        }
    }
}