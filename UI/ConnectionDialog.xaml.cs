using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using LINQPad.Extensibility.DataContext;
using MessageBox = System.Windows.MessageBox;

namespace OData4.UI
{
    public partial class ConnectionDialog
    {
        public ConnectionDialog(IConnectionInfo cxInfo)
        {
            InitializeComponent();
            DataContext = new ConnectionProperties(cxInfo);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.SourceInitialized"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;

            // Change the window style to remove icon and buttons
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));

            base.OnSourceInitialized(e);
        }

        #region p/invoke

        [DllImport("user32.dll")]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_STYLE = -16;

        private const uint WS_SYSMENU = 0x80000;

        #endregion

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void WebProxySetup(object sender, RoutedEventArgs e)
        {
            try
            {
                var type = Assembly.GetAssembly(typeof(IConnectionInfo)).GetType("LINQPad.UI.ProxyForm");

                var ci = type.GetConstructor(new Type[] { });

                using (var form = (Form)ci.Invoke(null))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't show LINQPad Web proxy settings.\r\n" +
                                "Open it manually from Edit->Preferences->Web proxy.\r\n" +
                                $"{ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
