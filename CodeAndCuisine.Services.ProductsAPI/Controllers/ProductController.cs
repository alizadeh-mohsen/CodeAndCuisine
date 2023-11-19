using AutoMapper;
using CodeAndCuisine.Services.ProductsAPI.Data;
using CodeAndCuisine.Services.ProductsAPI.Model.Model;
using CodeAndCuisine.Services.ProductsAPI.Model.Model.Dto;
using CodeAndCuisine.Services.ProductsAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeAndCuisine.Services.ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _dbContext;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;
        public ProductController(ProductDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _responseDto = new ResponseDto
            {
                IsSuccess = true,
                Message = string.Empty
            };
        }

        [HttpGet("{id}")]
        public ResponseDto Get(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                _responseDto.Result = null;
                _responseDto.Message = "Product not found";
                _responseDto.IsSuccess = false;
            }
            else
            {
                _responseDto.Result = _mapper.Map<ProductDto>(product);
                _responseDto.IsSuccess = true;
            }
            return _responseDto;
        }

        [HttpGet]
        public ResponseDto Get()
        {
            var products = _dbContext.Products.ToList();
            if (!products.Any())
            {
                _responseDto.Result = null;
                _responseDto.Message = "No product found";
                _responseDto.IsSuccess = false;
            }
            else
            {
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDto AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(product);
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = CreateErrorMessage(ex);
            }
            return _responseDto;


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ResponseDto UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();

                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = CreateErrorMessage(ex);
            }
            return _responseDto;

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ResponseDto DeleteProduct(int id)
        {
            try
            {
                var product = _dbContext.Products.Find(id);
                if (product == null)
                {
                    _responseDto.Message = "Product not found";
                    _responseDto.IsSuccess = false;
                }

                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = CreateErrorMessage(ex);
            }

            return _responseDto;
        }

        private string CreateErrorMessage(Exception ex)
        {
            return ex.Message + ex.InnerException == null ? string.Empty : ex.InnerException.Message;
        }

    }
}
