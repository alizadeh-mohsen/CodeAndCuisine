using System.Security.Cryptography.X509Certificates;

namespace CodeAndCuisine.Web.Utility
{
    public class StaticData
    {
        public const string Admin = "Admin"; 
        public const string Customer = "Customer"; 
        public static string CouponApiBase { get; set; }
        public static string AuthApiBase { get; set; }


        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE

        }
    }
}
