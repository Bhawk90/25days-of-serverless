using Newtonsoft.Json;

namespace GuatemalaCleansing.Models.Responses.Photos
{
    public class UnsplashPhotoUrls
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("full")]
        public string Full { get; set; }

        [JsonProperty("regular")]
        public string Regular { get; set; }

        [JsonProperty("small")]
        public string Small { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }
}