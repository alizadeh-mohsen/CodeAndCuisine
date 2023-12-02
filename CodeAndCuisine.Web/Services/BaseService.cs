using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using Newtonsoft.Json;
using System.Text;
using static CodeAndCuisine.Web.Utility.StaticData;

namespace CodeAndCuisine.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProviderService _tokenProviderService;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProviderService tokenProviderService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProviderService = tokenProviderService;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {

                HttpClient client = _httpClientFactory.CreateClient();

                var message = new HttpRequestMessage();
                HttpResponseMessage apiResponse = null;

                message.Headers.Add("Accept", "application/json");

                if (withBearer)
                {
                    var token = _tokenProviderService.GetToken();
                    message.Headers.Add("Authorization", $" Bearer {token}");

                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                 apiResponse = await client.SendAsync(message);
                if (apiResponse != null && apiResponse.IsSuccessStatusCode)
                {
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    if (apiResponseDto != null)
                    {
                        apiResponseDto.StatusCode = apiResponse.StatusCode;
                        return apiResponseDto;
                    }
                }

                return new ResponseDto { IsSuccess = false, Message = string.IsNullOrEmpty(apiResponse.ReasonPhrase) ? apiResponse.ReasonPhrase : apiResponse.StatusCode.ToString() };

            }

            catch (Exception ex)
            {

                var dto = new ResponseDto
                {

                    Message = ex.Message + " >>" + ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                    IsSuccess = false,
                };
                return dto;
            }
        }
    }
}
