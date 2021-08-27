using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using twitch_notify_v2.Models;

namespace twitch_notify_v2.Repositories
{
    class StreamerRepository
    {
        private static StreamerRepository _instance;
        public static StreamerRepository Instance
        {
            get { if (_instance == null) { _instance = new StreamerRepository(); } return _instance; }
        }

        private List<Streamer> _streamers;
        public List<Streamer> Streamers
        {
            get { return _streamers; }
            private set { _streamers = value; }
        }

        public List<string> GetStreamerNames()
        {
            if (Streamers == null)
            {
                LoadStreamers();
            }

            List<string> streamerNames = new List<String>();

            if (Streamers != null)
            {
                foreach (Streamer streamer in Streamers)
                {
                    streamerNames.Add(streamer.Name);
                }
            }

            return streamerNames;
        }

        public Streamer GetStreamer(string name)
        {
            Streamer result = _streamers.Find(streamer => streamer.Name == name);
            return result;
        }

        public List<Streamer> GetLiveStreamers()
        {
            List<Streamer> liveStreamers = new List<Streamer>();

            foreach (Streamer streamer in _streamers)
            {
                if (streamer.OnlineStatus)
                {
                    liveStreamers.Add(streamer);
                }
            }

            return liveStreamers;
        }

        public void LoadStreamers()
        {
            if (Streamers == null)
            {
                // Determine resource paths
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string specificFolder = Path.Combine(folder, "TwitchNotifier");
                string resourceName = "streamerList.json";
                string resourceLocation = Path.Combine(specificFolder, resourceName);

                if (File.Exists(resourceLocation))
                {
                    using (StreamReader file = File.OpenText(resourceLocation))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Streamers = (List<Streamer>)serializer.Deserialize(file, typeof(List<Streamer>));
                        file.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Error: File at location: " + resourceLocation + " does not exist!");
                }
            }
        }
        public void SaveStreamers()
        {
            if (_streamers != null)
            {
                // Determine resource paths
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string specificFolder = Path.Combine(folder, "TwitchNotifier");
                string resourceName = "streamerList.json";
                string resourceLocation = Path.Combine(specificFolder, resourceName);

                // Make sure the directory exists
                Directory.CreateDirectory(specificFolder);

                // Write to file
                using (StreamWriter file = File.CreateText(resourceLocation))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Streamers);
                    file.Close();
                    file.Dispose();
                }
            }
        }

        public void AddStreamer(Streamer streamer)
        {
            if (Streamers == null) Streamers = new List<Streamer>();
            Streamers.Add(streamer);
            SaveStreamers();
        }

        public void RemoveStreamer(Streamer streamer)
        {
            if (Streamers != null)
            {
                Streamers.Remove(streamer);
                SaveStreamers();
            }
        }
    }
}
