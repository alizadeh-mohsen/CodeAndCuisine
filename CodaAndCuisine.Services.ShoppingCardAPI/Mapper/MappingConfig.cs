using AutoMapper;
using CodaAndCuisine.Services.ShoppingCartAPI.Model;
using CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto;


namespace CodeAndCuisine.Services.ShoppingCartAPI.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailDto>().ReverseMap();

            });
            return mappingConfig;
        }
    }
}
