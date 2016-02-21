using System;
using System.Net;
using System.Xml.Linq;
using LINQPad.Extensibility.DataContext;

namespace OData4.UI
{
    public class ConnectionProperties
    {
        private readonly IConnectionInfo _connectionInfo;
        private readonly XElement _driverData;

        public ConnectionProperties()
        {
        }

        public ConnectionProperties(IConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
            _driverData = connectionInfo.DriverData;
        }

        public bool Persist
        {
            get { return _connectionInfo.Persist; }
            set { _connectionInfo.Persist = value; }
        }

        public string Uri
        {
            get { return (string)_driverData.Element("Uri"); }
            set { _driverData.SetElementValue("Uri", value); }
        }

        public string Domain
        {
            get { return (string)_driverData.Element("Domain"); }
            set { _driverData.SetElementValue("Domain", value); }
        }

        public string UserName
        {
            get { return (string)_driverData.Element("UserName"); }
            set { _driverData.SetElementValue("UserName", value); }
        }

        public string Password
        {
            get { return _connectionInfo.Decrypt((string)_driverData.Element("Password") ?? ""); }
            set { _driverData.SetElementValue("Password", _connectionInfo.Encrypt(value)); }
        }

        public bool UseProxy
        {
            get { return (bool?)_driverData.Element("UseProxy") ?? false; }
            set { _driverData.SetElementValue("UseProxy", value.ToString()); }
        }
        
        public AuthenticationType AuthenticationType
        {
            get { return (AuthenticationType?)(int?) _driverData.Element("AuthenticationType") ?? AuthenticationType.None; }
            set { _driverData.SetElementValue("AuthenticationType", (int)value);}
        }

        public ICredentials GetCredentials()
        {
            switch (AuthenticationType)
            {
                case AuthenticationType.None:
                    return null;
                case AuthenticationType.Windows:
                    return CredentialCache.DefaultCredentials;
                case AuthenticationType.Basic:
                    return !string.IsNullOrEmpty(UserName) 
                                        ? new NetworkCredential(UserName, Password, Domain ?? string.Empty) 
                                        : CredentialCache.DefaultNetworkCredentials;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IWebProxy GetWebProxy()
        {
            return UseProxy ? LINQPad.Util.GetWebProxy() : null;
        }
    }
}