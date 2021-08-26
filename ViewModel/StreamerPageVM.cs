using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Models;
using twitch_notify_v2.Repositories;

namespace twitch_notify_v2.ViewModel
{
    class StreamerPageVM: ViewModelBase
    {
        private List<Streamer> _streamers;
        public List<Streamer> Streamers
        {
            get { return _streamers; }
            set { _streamers = value; }
        }

        public StreamerPageVM()
        {
            Streamers = StreamerRepository.Instance.Streamers;
            RaisePropertyChanged("Streamers");
        }
    }
}
