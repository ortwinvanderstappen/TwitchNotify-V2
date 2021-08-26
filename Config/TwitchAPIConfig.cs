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

        private TwitchAPI _twitchAPI;
        public TwitchAPI TwitchAPI
        {
            get { return _twitchAPI; }
            private set { _twitchAPI = value; }
        }

        public TwitchAPIConfig()
        {
            _twitchAPI = new TwitchAPI();
            SetupCredentials();
        }

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

        private void SetupCredentials()
        {
            // Determine resource paths
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "TwitchNotifier");
            string resourceName = "credentials.json";
            string resourceLocation = Path.Combine(specificFolder, resourceName);

            if (File.Exists(resourceLocation))
            {
                using (StreamReader file = File.OpenText(resourceLocation))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    Credential credential = (Credential)serializer.Deserialize(file, typeof(Credential));
                    TwitchAPI.Settings.ClientId = credential.ClientId;
                    TwitchAPI.Settings.AccessToken = credential.AccessToken;

                    Console.WriteLine ( "clientid: " + credential.ClientId);
                }
            }
            else
            {
                Console.WriteLine("Error: File at location: " + resourceLocation + " does not exist!");
            }
        }
    }
}
