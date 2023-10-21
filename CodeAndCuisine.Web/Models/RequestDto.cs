
using static CodeAndCuisine.Web.Utility.StaticData;

namespace CodeAndCuisine.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = Utility.StaticData.ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
