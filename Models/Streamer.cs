using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitch_notify_v2.Models
{
    class Streamer
    {
        private string _name;
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _title;
        [JsonIgnore]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _game;
        [JsonIgnore]
        public string Game
        {
            get { return _game; }
            set { _game = value; }
        }


        private DateTime _startedAt;
        [JsonIgnore]
        public DateTime StartedAt
        {
            get { return _startedAt; }
            set { _startedAt = value; }
        }

        private string _avatarUrl;
        [JsonProperty(PropertyName = "avatarUrl")]
        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { _avatarUrl = value; }
        }

        private bool _onlineStatus = false;
        [JsonIgnore]
        public bool OnlineStatus
        {
            get { return _onlineStatus; }
            set { _onlineStatus = value; }
        }
    }
}
