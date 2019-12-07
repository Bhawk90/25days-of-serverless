using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GuatemalaCleansing.Models.Requests.Photos
{
    public class SearchPhotoRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; } = "";

        [JsonProperty("page")]
        public int PageNumber { get; set; } = 1;

        [JsonProperty("per_page")]
        public int ItemsPerPage { get; set; } = 10;

        [JsonProperty("collections")]
        public string Collections { get; set; } = "";

        [JsonProperty("orientation")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PhotoOrientation Orientation { get; set; }

        public enum PhotoOrientation
        {
            [EnumMember(Value = "landscape")]
            Landscape,
            [EnumMember(Value = "portrait")]
            Portrait,
            [EnumMember(Value = "squarish")]
            Squarish
        }
    }
}