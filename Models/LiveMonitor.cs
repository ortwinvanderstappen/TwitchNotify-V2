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
        private static LiveMonitor _instance = null;
        private static readonly object padlock = new object();
        public static LiveMonitor Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new LiveMonitor();
                    }
                    return _instance;
                }
            }
        }

        private LiveStreamMonitorService _monitor;

        public LivePageVM LivePageVM { get; set; }

        private bool _isStarted;

        public LiveMonitor()
        {
        }

        public void StartMonitor()
        {
            Console.WriteLine("Livemonitor: starting...");
            Task.Run(() => SetupLiveMonitor());
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

        public async void RefreshStreamerList()
        {
            // Set channels
            List<String> streamerNames = StreamerRepository.Instance.GetStreamerNames();

            // Only set the streamer names if any streamers are listed
            if (streamerNames.Count > 0)
            {
                _monitor.SetChannelsByName(streamerNames);
                var credentialResult = await TwitchAPIConfig.Instance.TwitchAPI.V5.Auth.CheckCredentialsAsync();

                // Start the service
                if (!_isStarted && credentialResult.Result)
                {
                    Console.WriteLine("Livemonitor: Started");

                    _monitor.Start();
                    await Task.Run(() => _monitor.UpdateLiveStreamersAsync());
                }
            }
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

            LivePageVM.RefreshStreamers();
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

            LivePageVM.RefreshStreamers();
        }

        private void OnServiceStarted(object sender, OnServiceStartedArgs e)
        {
            Console.WriteLine("Livemonitor: Service started");
            _isStarted = true;
        }
    }
}
