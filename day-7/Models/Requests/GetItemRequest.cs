using Newtonsoft.Json;

namespace GuatemalaCleansing.Models.Requests
{
    public class GetItemRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}