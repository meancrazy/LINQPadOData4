using System.Windows;

namespace OData4.LINQPadDriver
{
	public partial class ConnectionDialog
	{
		private readonly ConnectionProperties _connectionProperties;

		public ConnectionDialog(ConnectionProperties connectionProperties)
		{
			DataContext = _connectionProperties = connectionProperties;

			InitializeComponent();

			PasswordBox.Password = _connectionProperties.Password;
		}

		private void OnOkClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
		{
			_connectionProperties.Password = PasswordBox.Password;
		}
	}
}
