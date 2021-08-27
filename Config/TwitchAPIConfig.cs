using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Models;
using TwitchLib.Api;

namespace twitch_notify_v2.Config
{
    public sealed class TwitchAPIConfig
    {
        private static TwitchAPIConfig _instance = null;
        private static readonly object padlock = new object();
        public static TwitchAPIConfig Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TwitchAPIConfig();
                    }
                    return _instance;
                }
            }
        }

        public TwitchAPIConfig()
        {
            _twitchAPI = new TwitchAPI();
            //LoadCredentials();
        }

        private TwitchAPI _twitchAPI;
        public TwitchAPI TwitchAPI
        {
            get { return _twitchAPI; }
            private set { _twitchAPI = value; }
        }

        //public void LoadCredentials()
        //{
        //    // Determine resource paths
        //    string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    string specificFolder = Path.Combine(folder, "TwitchNotifier");
        //    string resourceName = "config.json";
        //    string resourceLocation = Path.Combine(specificFolder, resourceName);

        //    if (File.Exists(resourceLocation))
        //    {
        //        using (StreamReader file = File.OpenText(resourceLocation))
        //        {
        //            JsonSerializer serializer = new JsonSerializer();

        //            Models.Config credential = (Models.Config)serializer.Deserialize(file, typeof(Models.Config));

        //            Models.Config.Instance.ClientId = credential.ClientId;
        //            Models.Config.Instance.AccessToken = credential.AccessToken;
        //            Models.Config.Instance.StartWithWindows = credential.StartWithWindows;
        //            Models.Config.Instance.MinimizeToTray = credential.MinimizeToTray;
        //            Models.Config.Instance.ClickAlertToShowTwitch = credential.ClickAlertToShowTwitch;
        //            Models.Config.Instance.AlertDuration = credential.AlertDuration;

        //            TwitchAPI.Settings.ClientId = credential.ClientId;
        //            TwitchAPI.Settings.AccessToken = credential.AccessToken;
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Error: File at location: " + resourceLocation + " does not exist!");
        //    }
        //}

        public async Task<bool> VerifyCredentials()
        {
            var credentialResult = await Config.TwitchAPIConfig.Instance.TwitchAPI.V5.Auth.CheckCredentialsAsync();
            return credentialResult.Result;
        }
    }
}
