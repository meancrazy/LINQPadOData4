using Microsoft.OData.Client;

namespace OData4.LINQPadDriver
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
