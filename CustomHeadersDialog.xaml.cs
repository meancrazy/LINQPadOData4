using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace OData4.LINQPadDriver
{
	/// <summary>
	/// Interaction logic for CustomHeadersDialog.xaml
	/// </summary>
	public partial class CustomHeadersDialog : Window
	{
		public ObservableCollection<CustomHeader> CustomHeaders { get; set; } = new ObservableCollection<CustomHeader>();
		public CustomHeadersDialog(IEnumerable<KeyValuePair<string, string>> customHeaders)
		{
			foreach (var item in customHeaders)
			{
				CustomHeaders.Add(new CustomHeader { Name = item.Key, Value = item.Value });
			}
			InitializeComponent();
			DataContext = this;

		}

		public CustomHeader SelectedCustomHeader { get; set; }

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			this.Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			this.Close();
		}

		private void Remove_Click(object sender, RoutedEventArgs e)
		{
			if (SelectedCustomHeader != null && CustomHeaders.Contains(SelectedCustomHeader))
			{
				CustomHeaders.Remove(SelectedCustomHeader);
			}
		}
	}

	public class CustomHeader
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}
}
