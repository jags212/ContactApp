using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactApp.Data.Models
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("expiration")]
        public string expiration { get; set; }
    }
}
