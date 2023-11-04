using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using Newtonsoft.Json;
using System;
using System.Text;
using static CodeAndCuisine.Web.Utility.StaticData;

namespace CodeAndCuisine.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {

                HttpClient client = _httpClientFactory.CreateClient();

                var message = new HttpRequestMessage();
                HttpResponseMessage apiResponse = null;

                message.Headers.Add("Accept", "application/json");

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

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                apiResponseDto.StatusCode = apiResponse.StatusCode;
                return apiResponseDto;

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        {
                            return apiResponseDto;
                        }
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            Message = "Unauthorized"
                        };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            Message = "Forbidden"
                        };
                    case System.Net.HttpStatusCode.NotFound:
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            Message = "NotFound"
                        };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            Message = "InternalServerError"
                        };
                    //case System.Net.HttpStatusCode.:
                    //    return new ResponseDto
                    //    {
                    //        IsSuccess = false,
                    //        Message = "InternalServerError"
                    //    };
                    default:
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            Message = apiResponse.ReasonPhrase != null ? apiResponse.ReasonPhrase : apiResponse.StatusCode.ToString()
                        };

                }
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
