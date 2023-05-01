using LINQPad.Extensibility.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace OData4.LINQPadDriver
{
	public class ConnectionProperties
	{
		private readonly IConnectionInfo _connectionInfo;
		private readonly XElement _driverData;

		public ConnectionProperties(IConnectionInfo connectionInfo)
		{
			_connectionInfo = connectionInfo;
			_driverData = connectionInfo.DriverData;
			RedirectUri = "urn:ietf:wg:oauth:2.0:oob";//default
		}

		public bool Persist
		{
			get => _connectionInfo.Persist;
			set => _connectionInfo.Persist = value;
		}

		public string Uri
		{
			get => (string)_driverData.Element("Uri");
			set => _driverData.SetElementValue("Uri", value);
		}

		public string Domain
		{
			get => (string)_driverData.Element("Domain");
			set => _driverData.SetElementValue("Domain", value);
		}

		public string UserName
		{
			get => (string)_driverData.Element("UserName");
			set => _driverData.SetElementValue("UserName", value);
		}

		public string Password
		{
			get => _connectionInfo.Decrypt((string)_driverData.Element("Password") ?? "");
			set => _driverData.SetElementValue("Password", _connectionInfo.Encrypt(value));
		}

		public bool UseProxy
		{
			get => (bool?)_driverData.Element("UseProxy") ?? false;
			set => _driverData.SetElementValue("UseProxy", value.ToString());
		}

		public bool AcceptInvalidCertificate
		{
			get => (bool?)_driverData.Element("AcceptInvalidCertificate") ?? false;
			set => _driverData.SetElementValue("AcceptInvalidCertificate", value.ToString());
		}

		public AuthenticationType AuthenticationType
		{
			get => (AuthenticationType?)(int?)_driverData.Element("AuthenticationType") ?? AuthenticationType.None;
			set => _driverData.SetElementValue("AuthenticationType", (int)value);
		}

		public bool LogMethod
		{
			get => (bool?)_driverData.Element("LogMethod") ?? true;
			set => _driverData.SetElementValue("LogMethod", value.ToString());
		}

		public bool LogHeaders
		{
			get => (bool?)_driverData.Element("LogHeaders") ?? true;
			set => _driverData.SetElementValue("LogHeaders", value.ToString());
		}

		public IEnumerable<KeyValuePair<string, string>> CustomHeaders
		{
			get
			{
				var element = _driverData.Element("CustomHeaders");
				if (element == null)
					return new KeyValuePair<string, string>[0];

				return element.Elements("Header")
					.Select(x => new KeyValuePair<string, string>((string)x.Attribute("Name") ?? "",
						(string)x.Attribute("Value") ?? ""))
					.Where(x => !string.IsNullOrWhiteSpace(x.Key));
			}
			set
			{
				var headers = value?.Where(x => !string.IsNullOrWhiteSpace(x.Key))
					.Select(x => new XElement("Header",
						new XAttribute("Name", x.Key.Trim()),
						new XAttribute("Value", (x.Value ?? "").Trim())));

				_driverData.Elements("CustomHeaders").Remove();
				_driverData.Add(new XElement("CustomHeaders", headers));
			}
		}

		public string ClientCertificateFile
		{
			get => (string)_driverData.Element("ClientCertificateFile");
			set => _driverData.SetElementValue("ClientCertificateFile", value);
		}


		public string Authority
		{
			get { return (string)_driverData.Element("Authority"); }
			set { _driverData.SetElementValue("Authority", value); }
		}

		public string ApplicationId
		{
			get { return (string)_driverData.Element("ApplicationId"); }
			set { _driverData.SetElementValue("ApplicationId", value); }
		}
		public string Scopes
		{
			get { return (string)_driverData.Element("Scopes"); }
			set { _driverData.SetElementValue("Scopes", value); }
		}
		public string RedirectUri
		{
			get { return (string)_driverData.Element("RedirectUri"); }
			set { _driverData.SetElementValue("RedirectUri", value); }
		}

		public ICredentials GetCredentials()
		{
			return AuthenticationType switch
			{
				AuthenticationType.None => null,
				AuthenticationType.Windows => CredentialCache.DefaultCredentials,
				AuthenticationType.Basic => !string.IsNullOrEmpty(UserName)
										? new NetworkCredential(UserName, Password, Domain ?? string.Empty)
										: CredentialCache.DefaultNetworkCredentials,
				AuthenticationType.ClientCertificate => null,
				AuthenticationType.AzureAD => null,
				_ => throw new NotImplementedException()
			};
		}

		public IWebProxy GetWebProxy()
		{
			var proxy = WebRequest.GetSystemWebProxy();

			if (UseProxy)
			{
				proxy = LINQPad.Util.GetIWebProxy() ?? proxy;
			}

			return proxy;
		}

		public NameValueCollection GetCustomHeaders()
		{
			var headers = new NameValueCollection();
			foreach (var header in CustomHeaders)
			{
				headers.Add(header.Key, header.Value);
			}

			return headers;
		}
	}
}
