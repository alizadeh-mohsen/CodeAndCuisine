using System.Security.Cryptography.X509Certificates;

namespace CodeAndCuisine.Web.Utility
{
    public class StaticData
    {
        public static string CouponApiBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE

        }
    }
}
