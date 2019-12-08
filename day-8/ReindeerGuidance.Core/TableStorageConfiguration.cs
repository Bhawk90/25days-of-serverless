using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReindeerGuidance.Core
{
    public class TableStorageConfiguration
    {
        [JsonProperty("StorageName")]
        public string StorageName { get; set; }

        [JsonProperty("AccessKey")]
        public string AccessKey { get; set; }
    }
}
