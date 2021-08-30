using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Minimize to tray logic: https://possemeeg.wordpress.com/2007/09/06/minimize-to-tray-icon-in-wpf/

namespace twitch_notify_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;
        private WindowState _storedWindowState = WindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize tray icon
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "Twitch live notifier";
            Stream iconStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Icons/twitch_live_notifier_icon.ico")).Stream;
            _notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            _notifyIcon.Click += new EventHandler(NotifyIconClick);
        }

        void OnClose(object sender, CancelEventArgs args)
        {
            // Close the application and dispose of the tray icon
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        void OnStateChanged(object sender, EventArgs args)
        {
            // Ignore if minimize to tray is not active
            if(!Models.Config.Instance.MinimizeToTray) return;

            // Handle changed window state
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
            else
            {
                _storedWindowState = WindowState;
            }
        }

        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            // Ignore if minimize to tray is not active
            if(!Models.Config.Instance.MinimizeToTray) return;

            // Handle tray icon visibility
            if (_notifyIcon != null)
                _notifyIcon.Visible = !IsVisible;
        }

        private void NotifyIconClick(object sender, EventArgs args)
        {
            // Handle tray icon click event
            Show();
            WindowState = _storedWindowState;
        }
    }
}
