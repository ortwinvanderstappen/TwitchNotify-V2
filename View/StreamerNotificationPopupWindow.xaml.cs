using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace twitch_notify_v2.View
{
    /// <summary>
    /// Interaction logic for StreamerNotificationPopupWindow.xaml
    /// </summary>
    public partial class StreamerNotificationPopupWindow : Window
    {
        public StreamerNotificationPopupWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs args)
        {
            var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.Left = workingArea.Right - this.ActualWidth;
            this.Top = workingArea.Bottom - this.ActualHeight;
        }

        private void WindowRightButtonDown(object sender, EventArgs args)
        {
            this.Close();
        }
    }
}
