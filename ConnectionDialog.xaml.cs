using System;
using System.Collections.Generic;
using System.Linq;
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

		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				CustomHeadersDialog customHeadersDialog = new CustomHeadersDialog(_connectionProperties.CustomHeaders);
				var dialogResult = customHeadersDialog.ShowDialog();
				if (dialogResult == true)
				{
					var customeHeaders = new List<KeyValuePair<string, string>>();
					foreach (var item in customHeadersDialog.CustomHeaders.Where(s => !string.IsNullOrEmpty(s.Name)))
					{
						customeHeaders.Add(new KeyValuePair<string, string>(item.Name, item.Value));
					}
					_connectionProperties.CustomHeaders = customeHeaders;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Something went wrong \n{ex.Message}. Please try again!");
			}
		}
	}
}
