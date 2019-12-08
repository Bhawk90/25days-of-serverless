using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace ReindeerGuidance.Core.Data
{
    public class IncidentEntity : TableEntity
    {
        private IncidentStatus _status;
        [JsonIgnore]
        [IgnoreProperty]
        public IncidentStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                StatusProperty = value.ToString();
            }
        }

        [JsonProperty("status")]
        public string StatusProperty 
        {
            get;
            set;
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("modifiedAt")]
        public DateTimeOffset ModifiedAt { get; set; }

        public enum IncidentStatus
        {
            [EnumMember(Value = "open")]
            Open,
            [EnumMember(Value = "closed")]
            Closed,
            [EnumMember(Value = "inprogress")]
            InProgress
        }
    }
}
