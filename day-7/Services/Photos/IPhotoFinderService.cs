using System.Threading.Tasks;
using GuatemalaCleansing.Models.Requests.Photos;
using GuatemalaCleansing.Models.Responses.Photos;

namespace GuatemalaCleansing.Services.Photos
{
    public interface IPhotoFinderService
    {
        Task<UnsplashResponse<SearchPhotoResponse>> SearchPhotoAsync(SearchPhotoRequest request);
    }
}