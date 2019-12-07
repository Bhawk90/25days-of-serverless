using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GuatemalaCleansing.Models.Requests;
using GuatemalaCleansing.Services.Photos;
using System.Linq;
using GuatemalaCleansing.Factories;

namespace GuatemalaCleansing
{
    public static class GuatemalaCleansingFunctions
    {
        #region Functions

        [FunctionName("GetItemRequest")]
        public static async Task<IActionResult> GetItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var request = JsonConvert.DeserializeObject<GetItemRequest>(await req.ReadAsStringAsync());
            if (request == null || String.IsNullOrWhiteSpace(request.Query))
            {
                return new BadRequestObjectResult($"A 'query' parameter in body is required!");
            }

            // Retrieve images from Unsplash and return the one with the highest number of likes
            IPhotoFinderService photoFinder = new PhotoFinderService();
            var photos = await photoFinder.SearchPhotoAsync(new Models.Requests.Photos.SearchPhotoRequest()
            {
                Query = request.Query
            });

            var highestRankingPhoto = photos.Results.OrderByDescending(p => p.Likes).FirstOrDefault();
            if (highestRankingPhoto != null)
            {
                return new OkObjectResult(UnsplashPhotoFactory.UnsplashPhotoToItemResponse(highestRankingPhoto));
            }
            else
            {
                return new NotFoundResult();
            }
        }

        #endregion Functions

    }
}
