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
                //.ForMember(dest => dest.Description,
                //    opt => opt.MapFrom(src => src.Description.Length > 50 ? src.Description.Substring(0, 50) : src.Description)
                //    );
                //config.CreateMap<ProductDto, Product>();
            });
            return mappingConfig;
        }
    }
}
