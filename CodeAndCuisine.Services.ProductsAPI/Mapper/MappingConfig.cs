using AutoMapper;
using CodeAndCuisine.Services.ProductsAPI.Model.Model;
using CodeAndCuisine.Services.ProductsAPI.Model.Model.Dto;

namespace CodeAndCuisine.Services.ProductsAPI.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
