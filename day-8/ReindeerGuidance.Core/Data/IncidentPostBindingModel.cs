using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReindeerGuidance.Core.Data
{
    public class IncidentPostBindingModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}
