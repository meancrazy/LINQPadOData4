using System.Linq;
using Microsoft.OData.Client;
using Azure.Identity;
using Azure.Core;
using System;

namespace OData4.LINQPadDriver
{
	public class CustomizedRequestMessage : HttpWebRequestMessage
	{
		static InteractiveBrowserCredential credential;
		public CustomizedRequestMessage(DataServiceClientRequestMessageArgs args, ConnectionProperties properties) : base(args)
		{
			HttpWebRequest.Proxy = properties.GetWebProxy();
			HttpWebRequest.Headers.Add(properties.GetCustomHeaders());

			var cert = properties.GetClientCertificate();
			if (cert != null)
			{
				HttpWebRequest.ClientCertificates.Add(cert);
			}

			if (properties.AuthenticationType == AuthenticationType.AzureAD)
			{
				credential ??= new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
				{
					TenantId = properties.Authority,
					ClientId = properties.ApplicationId,
					RedirectUri = new Uri(properties.RedirectUri),
				});

				var token = credential.GetToken(new TokenRequestContext(properties.Scopes.Split(',').ToArray()));

				HttpWebRequest.Headers.Set("Authorization", $"Bearer {token.Token}");
			}
		}
	}
}
