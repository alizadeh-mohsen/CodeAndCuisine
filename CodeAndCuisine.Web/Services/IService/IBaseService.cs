using CodeAndCuisine.Web.Models;

namespace CodeAndCuisine.Web.Services.IService
{
    public interface IBaseService
    {
       Task<ResponseDto> SendAsync(RequestDto requestDto);
    }
}
