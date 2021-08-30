using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Models;

namespace twitch_notify_v2.ViewModel
{
    class StreamerNotificationPopupWindowViewModel : ObservableObject
    {
        private Streamer _streamer;
        public Streamer Streamer
        {
            get { return _streamer; }
            set
            {
                _streamer = value;
                RaisePropertyChanged("StreamerIcon");
                RaisePropertyChanged("StreamerTitle");
                RaisePropertyChanged("StreamerGame");
                RaisePropertyChanged("StreamerUptime");
            }
        }

        public string StreamerIcon { get { if (Streamer == null) return ""; return Streamer.AvatarUrl; } }

        public string StreamerTitle { get { if (Streamer == null) return ""; return Streamer.Name + " - " + Streamer.Title; } }
        public string StreamerGame { get { if (Streamer == null) return ""; return Streamer.Game; } }
        public string StreamerUptime { get { if (Streamer == null) return ""; return Streamer.StartedAt.ToString(); } }
    }
}
