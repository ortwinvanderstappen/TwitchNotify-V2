using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.View;
using twitch_notify_v2.ViewModel;

namespace twitch_notify_v2.Models
{
    class NotificationManager
    {
        private static NotificationManager _instance;
        public static NotificationManager Instance
        {
            get { if (_instance == null) { _instance = new NotificationManager(); } return _instance; }
        }

        StreamerNotificationPopupWindow _popupWindow;

        public void ShowLivestreamNotification(Streamer streamer)
        {
            // Create the window if it's null
            if (_popupWindow == null)
                _popupWindow = new StreamerNotificationPopupWindow();

            // Set streamer data in window...
            StreamerNotificationPopupWindowViewModel vm = (StreamerNotificationPopupWindowViewModel)_popupWindow.DataContext;
            vm.Streamer = streamer;

            // Show the window
            _popupWindow.Show();
        }
    }
}
