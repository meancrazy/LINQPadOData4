using System.Collections.Generic;
using System.Windows;

namespace OData4.LINQPadDriver
{
	/// <summary>
	/// Interaction logic for CustomHeadersDialog.xaml
	/// </summary>
	public partial class CustomHeadersDialog : Window
	{
		public List<CustomHeader> CustomHeaders { get; set; } = new List<CustomHeader>();
		public CustomHeadersDialog()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			CustomHeaders.Clear();
			this.Close();
		}
	}

	public class CustomHeader
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}
}
