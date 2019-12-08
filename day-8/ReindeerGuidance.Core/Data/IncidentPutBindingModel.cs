using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReindeerGuidance.Core.Data
{
    public class IncidentPutBindingModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
