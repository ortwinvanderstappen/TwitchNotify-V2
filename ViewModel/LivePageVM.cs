using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Models;
using twitch_notify_v2.Repositories;

namespace twitch_notify_v2.ViewModel
{
    class LivePageVM : ViewModelBase
    {
        private LiveMonitor _liveMonitor;
        public LiveMonitor LiveMonitor { get { return _liveMonitor; } private set { _liveMonitor = value; } }

        private List<Streamer> _liveStreamers;
        public List<Streamer> LiveStreamers { get { return _liveStreamers; } }

        public LivePageVM()
        {
            LoadStreamers();
        }

        private void LoadStreamers()
        {
            // Load list of streamers that needs to be monitored
            StreamerRepository.Instance.LoadStreamers();

            // Start the live monitor
            _liveMonitor = new LiveMonitor(this);
            _liveMonitor.StartMonitor();
        }

        public void RefreshStreamers()
        {
            Console.WriteLine("Refreshing live streamers");
            _liveStreamers = StreamerRepository.Instance.GetLiveStreamers();
            RaisePropertyChanged("LiveStreamers");
        }
    }
}
