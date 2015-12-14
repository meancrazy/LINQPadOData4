using Microsoft.OData.Client;

namespace OData4
{
    public class CustomizedRequestMessage : HttpWebRequestMessage
    {
        public CustomizedRequestMessage(DataServiceClientRequestMessageArgs args)
            : base(args)
        {
            HttpWebRequest.Proxy = LINQPad.Util.GetWebProxy();
        }
    }
}