using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Config;
using twitch_notify_v2.Models;
using twitch_notify_v2.Repositories;
using TwitchLib.Api.Core.Exceptions;

namespace twitch_notify_v2.ViewModel
{
    class StreamerPageVM : ViewModelBase
    {
        private ObservableCollection<Streamer> _streamers = new ObservableCollection<Streamer>();

        public ObservableCollection<Streamer> Streamers
        {
            get { return _streamers; }
            set { _streamers = value; }
        }

        public bool ShowAddStreamerPopup { get; set; }

        public StreamerPageVM()
        {
            RefreshStreamers();
        }

        public void RefreshStreamers()
        {
            List<Streamer> streamers = StreamerRepository.Instance.Streamers;
            if (streamers != null)
            {
                Streamers.Clear();
                foreach (Streamer s in streamers)
                {
                    Streamers.Add(s);
                }
                RaisePropertyChanged("Streamers");
            }
        }

        private RelayCommand _addStreamerCommand;
        public RelayCommand AddStreamerCommand { get { if (_addStreamerCommand == null) _addStreamerCommand = new RelayCommand(ShowAddStreamerPopupWindow); return _addStreamerCommand; } }

        private void ShowAddStreamerPopupWindow()
        {
            ShowAddStreamerPopup = true;
            RaisePropertyChanged("ShowAddStreamerPopup");
            RaisePropertyChanged("AddStreamerName");
        }

        private void CloseAddStreamerPopup()
        {
            ShowAddStreamerPopup = false;
            AddStreamerName = "";
            RaisePropertyChanged("ShowAddStreamerPopup");
            RaisePropertyChanged("AddStreamerName");
        }

        private RelayCommand _verifyStreamerCommand;
        public RelayCommand VerifyStreamerCommand { get { if (_verifyStreamerCommand == null) _verifyStreamerCommand = new RelayCommand(VerifyStreamer); return _verifyStreamerCommand; } }

        private RelayCommand _closeAddStreamerPopupCommand;
        public RelayCommand CloseAddStreamerPopupCommand { get { if (_closeAddStreamerPopupCommand == null) _closeAddStreamerPopupCommand = new RelayCommand(CloseAddStreamerPopup); return _closeAddStreamerPopupCommand; } }

        public string AddStreamerName { get; set; }
        private async void VerifyStreamer()
        {
            try
            {
                var user = await TwitchAPIConfig.Instance.TwitchAPI.V5.Users.GetUserByNameAsync(AddStreamerName);
                bool isOnline = await TwitchAPIConfig.Instance.TwitchAPI.V5.Streams.BroadcasterOnlineAsync(user.Matches[0].Id);

                if (user.Matches.Length >= 1)
                {
                    CloseAddStreamerPopup();

                    Streamer streamer = new Streamer();
                    streamer.AvatarUrl = user.Matches[0].Logo;
                    streamer.Name = user.Matches[0].DisplayName;
                    streamer.OnlineStatus = isOnline;

                    if (isOnline)
                    {
                        var stream = await TwitchAPIConfig.Instance.TwitchAPI.V5.Streams.GetStreamByUserAsync(user.Matches[0].Id);
                        streamer.Title = stream.Stream.Channel.Status;
                        streamer.StartedAt = stream.Stream.CreatedAt;
                        streamer.Game = stream.Stream.Game;
                    }

                    StreamerManager.Instance.AddStreamer(streamer);

                    RefreshStreamers();
                }
                else
                {
                    AddStreamerName = "";
                    RaisePropertyChanged("AddStreamerName");
                }

            }
            catch (BadParameterException)
            {
                AddStreamerName = "";
                RaisePropertyChanged("AddStreamerName");
            }
        }
    }
}
