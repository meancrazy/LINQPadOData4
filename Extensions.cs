using LINQPad.Extensibility.DataContext;
using OData4.UI;

namespace OData4
{
    public static class Extensions
    {
        public static ConnectionProperties GetConnectionProperties(this IConnectionInfo connectionInfo)
        {
            return new ConnectionProperties(connectionInfo);
        }
    }
}