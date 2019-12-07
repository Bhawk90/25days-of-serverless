using GuatemalaCleansing.Models.Responses;
using GuatemalaCleansing.Models.Responses.Photos;

namespace GuatemalaCleansing.Factories
{
    public static class UnsplashPhotoFactory
    {
        public static GetItemResponse UnsplashPhotoToItemResponse(SearchPhotoResponse item)
        {
            return new GetItemResponse()
            {
                Image = item.Urls.Raw
            };
        }
    }
}