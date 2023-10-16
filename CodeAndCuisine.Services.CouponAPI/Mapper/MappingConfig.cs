using AutoMapper;
using CodeAndCuisine.Services.CouponAPI.Models;
using CodeAndCuisine.Services.CouponAPI.Models.Dtos;

namespace CodeAndCuisine.Services.CouponAPI.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
