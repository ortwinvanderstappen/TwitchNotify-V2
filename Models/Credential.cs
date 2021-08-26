using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitch_notify_v2.Models
{
    class Credential
    {
        [JsonProperty(PropertyName = "ClientId")]
        public string ClientId { get; set; }
        [JsonProperty(PropertyName = "AccessToken")]
        public string AccessToken { get; set; }
    }
}
