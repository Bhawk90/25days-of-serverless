using System.Collections.Generic;
using Newtonsoft.Json;

namespace GuatemalaCleansing.Models.Responses.Photos
{
    public class UnsplashResponse<T> where T : UnsplashResponseItem
    {
        [JsonProperty("total")]
        public int TotalRecords { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public IList<T> Results { get; set; }
    }
}