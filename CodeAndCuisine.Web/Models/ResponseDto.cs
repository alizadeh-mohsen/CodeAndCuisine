using System.Net;

namespace CodeAndCuisine.Web.Models
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; } 
    }
}
