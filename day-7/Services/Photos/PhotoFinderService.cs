using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using GuatemalaCleansing.Models.Requests.Photos;
using GuatemalaCleansing.Models.Responses.Photos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GuatemalaCleansing.Services.Photos
{
    public class PhotoFinderService : IPhotoFinderService
    {
        #region Data Members

        private string _accessKey = "";
        private string _secretKey = "";

        #endregion Data Members

        #region Constructor

        public PhotoFinderService()
        {
            _accessKey = System.Environment.GetEnvironmentVariable("UnsplashAccessKey", EnvironmentVariableTarget.Process)
                ?? throw new ArgumentNullException("Environment::UnsplashAccessKey");

            _secretKey = System.Environment.GetEnvironmentVariable("UnsplashSecretKey", EnvironmentVariableTarget.Process)
                ?? throw new ArgumentNullException("Environment::UnsplashSecretKey");
        }

        #endregion Constructor

        #region Public Methods

        public async Task<UnsplashResponse<SearchPhotoResponse>> SearchPhotoAsync(SearchPhotoRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                // Set Authorization header
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Client-ID", _accessKey);

                // Build query string from request
                var jObject = JObject.FromObject(request);
                var queryString =
                    String.Join("&", jObject.Children().Cast<JProperty>()
                        .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.Value.ToString())}"));

                // Send request
                var response = await httpClient.GetAsync($"https://api.unsplash.com/search/photos?{queryString}");

                // Parse and return result
                return JsonConvert.DeserializeObject<UnsplashResponse<SearchPhotoResponse>>(await response.Content.ReadAsStringAsync());
            }
        }

        #endregion Public Methods
    }
}