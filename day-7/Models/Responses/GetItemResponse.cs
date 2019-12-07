using Newtonsoft.Json;

namespace GuatemalaCleansing.Models.Responses
{
    public class GetItemResponse
    {
        [JsonProperty("image")]
        public string Image { get; set; }
    }
}