using System.Net;
using Microsoft.OData.Client;

namespace OData4
{
    public class CustomizedRequestMessage : HttpWebRequestMessage
    {
        public CustomizedRequestMessage(DataServiceClientRequestMessageArgs args, IWebProxy webProxy) : base(args)
        {
            HttpWebRequest.Proxy = webProxy;
        }
    }
}