using System;
using Newtonsoft.Json;

namespace GuatemalaCleansing.Models.Responses.Photos
{
    public class SearchPhotoResponse : UnsplashResponseItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("urls")]
        public UnsplashPhotoUrls Urls { get; set; }
    }
}