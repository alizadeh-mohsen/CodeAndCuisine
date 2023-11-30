using AutoMapper;
using CodaAndCuisine.Services.ShoppingCartAPI.Model;
using CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto;
using CodeAndCuisine.Services.ShoppingCartAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ShoppingCartDbContext _context;
        private ResponseDto _responseDto;

        public ShoppingCartController(IMapper mapper, ShoppingCartDbContext context)
        {
            _mapper = mapper;
            _context = context;
            _responseDto = new ResponseDto
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = string.Empty
            };

        }
       
        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CardUpsert(ShoppingCartDto shoppingCartDto)
        {
            try
            {
                if (string.IsNullOrEmpty(shoppingCartDto.CartHeader.UserId))
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "User not found"
                    };
                
                var cartFromDb = await _context.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.UserId == shoppingCartDto.CartHeader.UserId);
                if (cartFromDb == null)
                {
                    var cartHeader = _mapper.Map<CartHeader>(shoppingCartDto.CartHeader);
                    _context.CartHeaders.Add(cartHeader);
                    await _context.SaveChangesAsync();

                    shoppingCartDto.CartDetails.First().CartHeaderId = cartHeader.Id;
                    _context.CartDetails.Add(_mapper.Map<CartDetail>(shoppingCartDto.CartDetails.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var detailFromDb = await _context.CartDetails.AsNoTracking()
                        .FirstOrDefaultAsync(d => d.ProductId == shoppingCartDto.CartDetails[0].ProductId &&
                        d.CartHeaderId == cartFromDb.Id);

                    if (detailFromDb == null)
                    {
                        shoppingCartDto.CartDetails.First().CartHeaderId = cartFromDb.Id;

                        _context.CartDetails.Add(_mapper.Map<CartDetail>(shoppingCartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        detailFromDb.Quantity += shoppingCartDto.CartDetails.First().Quantity;
                        _context.CartDetails.Update(detailFromDb);
                        await _context.SaveChangesAsync();
                    }
                }
                _responseDto.IsSuccess = true;
                _responseDto.Result = shoppingCartDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Result = ex.Message;

            }
            return _responseDto;
        }
    }
}
