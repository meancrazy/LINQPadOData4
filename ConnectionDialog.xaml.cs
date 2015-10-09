using System.Windows;
using LINQPad.Extensibility.DataContext;

namespace OData4
{
    public partial class ConnectionDialog
    {
        public ConnectionDialog(IConnectionInfo cxInfo)
        {
            DataContext = cxInfo;
            Background = SystemColors.ControlBrush;
            InitializeComponent();
        }

        void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
