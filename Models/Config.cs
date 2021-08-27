using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitch_notify_v2.Models
{
    class Config: ObservableObject
    {
        private static Config _instance;
        [JsonIgnore]
        public static Config Instance
        {
            get { if (_instance == null) { _instance = new Config(); } return _instance; }
        }

        private string _clientId;
        [JsonProperty(PropertyName = "ClientId")]
        public string ClientId { get{return _clientId; } set{ _clientId = value; RaisePropertyChanged("ClientId"); } }

        private string _accessToken;
        [JsonProperty(PropertyName = "AccessToken")]
        public string AccessToken { get{return _accessToken; } set{ _accessToken = value; RaisePropertyChanged("AccessToken"); } }
        
        [JsonProperty(PropertyName = "StartWithWindows")]
        public bool StartWithWindows { get; set; }
        
        [JsonProperty(PropertyName = "MinimizeToTray")]
        public bool MinimizeToTray { get; set; }
        
        [JsonProperty(PropertyName = "ClickAlertToShowTwitch")]
        public bool ClickAlertToShowTwitch { get; set; }
        
        [JsonProperty(PropertyName = "AlertDuration")]
        public int AlertDuration { get; set; } = 2;
    }
}
