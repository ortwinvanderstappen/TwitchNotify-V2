using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Config;
using twitch_notify_v2.Repositories;
using twitch_notify_v2.ViewModel;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLib.Api.V5.Models.Users;

namespace twitch_notify_v2.Models
{
    class LiveMonitor
    {
        private LiveStreamMonitorService _monitor;
        private LivePageVM _livePageVM;
        private bool _isStarted;

        public LiveMonitor(LivePageVM vm)
        {
            _livePageVM = vm;
        }

        public void StartMonitor()
        {
            Task.Run(() => SetupLiveMonitor());
        }

        public void RefreshStreamerList()
        {
            // Set channels
            List<String> streamerNames = StreamerRepository.Instance.GetStreamerNames();

            // Only set the streamer names if any streamers are listed
            if (streamerNames.Count > 0)
            {
                _monitor.SetChannelsByName(streamerNames);

                // Start the service
                if (!_isStarted)
                    _monitor.Start();

                Task.Run(() => _monitor.UpdateLiveStreamersAsync());
            }
        }

        private void SetupLiveMonitor()
        {
            // Setup monitor
            TwitchAPIConfig twitchAPIConfig = TwitchAPIConfig.Instance;
            _monitor = new LiveStreamMonitorService(twitchAPIConfig.TwitchAPI, 10);

            // Subscribe events
            _monitor.OnStreamOnline += OnStreamOnlineAsync;
            _monitor.OnStreamOffline += OnStreamOfflineAsync;
            _monitor.OnServiceStarted += OnServiceStarted;

            RefreshStreamerList();
        }

        private async void OnStreamOnlineAsync(object sender, OnStreamOnlineArgs args)
        {
            Users user = await TwitchAPIConfig.Instance.TwitchAPI.V5.Users.GetUserByNameAsync(args.Channel);

            Streamer streamer = StreamerRepository.Instance.GetStreamer(args.Channel);
            if (streamer != null)
            {
                streamer.AvatarUrl = user.Matches[0].Logo;
                streamer.OnlineStatus = true;
                streamer.StartedAt = args.Stream.StartedAt;
                streamer.Title = args.Stream.Title;
                streamer.Game = args.Stream.GameName;

                Console.WriteLine("Streamer " + args.Channel + " is now online: " + streamer.OnlineStatus);
            }

            StreamerRepository.Instance.SaveStreamers();
            _livePageVM.RefreshStreamers();
        }

        private async void OnStreamOfflineAsync(object sender, OnStreamOfflineArgs args)
        {
            Users user = await TwitchAPIConfig.Instance.TwitchAPI.V5.Users.GetUserByNameAsync(args.Channel);
            Streamer streamer = StreamerRepository.Instance.GetStreamer(args.Channel);
            if (streamer != null)
            {
                streamer.OnlineStatus = false;

                Console.WriteLine("Streamer " + args.Channel + " is now offline: " + streamer.OnlineStatus);
            }

            _livePageVM.RefreshStreamers();
        }

        private void OnServiceStarted(object sender, OnServiceStartedArgs e)
        {
            _isStarted = true;
        }
    }
}
