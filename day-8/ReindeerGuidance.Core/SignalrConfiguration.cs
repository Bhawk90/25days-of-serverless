using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReindeerGuidance.Core
{
    public class SignalrConfiguration
    {
        [JsonProperty("SIGNLR_HOST")]
        public string Host { get; set; }

        [JsonProperty("SIGNLR_ACCESSKEY")]
        public string AccessKey { get; set; }
    }
}
