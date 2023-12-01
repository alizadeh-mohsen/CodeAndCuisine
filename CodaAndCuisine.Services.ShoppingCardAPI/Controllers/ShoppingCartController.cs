using AutoMapper;
using CodaAndCuisine.Services.ShoppingCartAPI.Model;
using CodaAndCuisine.Services.ShoppingCartAPI.Model.Dto;
using CodaAndCuisine.Services.ShoppingCartAPI.Service.IService;
using CodeAndCuisine.Services.ShoppingCartAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodaAndCuisine.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ShoppingCartDbContext _context;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private ResponseDto _responseDto;

        public ShoppingCartController(IMapper mapper, ShoppingCartDbContext context, IProductService productService, ICouponService couponService)
        {
            _mapper = mapper;
            _context = context;
            _productService = productService;
            _couponService = couponService;
            _responseDto = new ResponseDto
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = string.Empty
            };

        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetShoppingCart(string userId)
        {
            var cartHeader = _context.CartHeaders.FirstOrDefault(c => c.UserId == userId);
            var cartDetails = _context.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id);

            foreach (var item in cartDetails)
            {
                item.Product = await _productService.GetProductById(item.ProductId);
                cartHeader.CartTotal += item.Quantity * item.Product.Price;
            }

            if (!string.IsNullOrEmpty(cartHeader.CouponCode))
            {
                var coupon = await _couponService.GetCouponByCode(cartHeader.CouponCode);
                if (coupon != null && cartHeader.CartTotal > coupon.MinAmount)
                {
                    cartHeader.CartTotal -= coupon.DiscountAmount;
                    cartHeader.Discount = coupon.DiscountAmount;
                }
            }

            var shoppoingCart = new ShoppingCartDto
            {

                CartHeader = _mapper.Map<CartHeaderDto>(cartHeader),
                CartDetails = _mapper.Map<List<CartDetailDto>>(cartDetails)
            };

            _responseDto.IsSuccess = true;
            _responseDto.Result = shoppoingCart;
            return _responseDto;
        }

        [HttpPost("Upsert")]
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
                        detailFromDb.Quantity = shoppingCartDto.CartDetails.First().Quantity;
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

        [HttpPost("Remove")]
        public async Task<ResponseDto> RemoveItem([FromBody] int detailId)
        {
            try
            {

                var detailToRemove = await _context.CartDetails.FirstOrDefaultAsync(c => c.Id == detailId);
                if (detailToRemove == null)
                {
                    _responseDto.IsSuccess = true;
                    return _responseDto;
                }



                var count = _context.CartDetails.AsNoTracking().Count(d => d.CartHeaderId == detailToRemove.CartHeaderId);

                if (count > 1)
                    _context.CartDetails.Remove(detailToRemove);
                else
                {
                    var cartFromDb = await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == detailToRemove.CartHeaderId);
                    _context.CartHeaders.Remove(cartFromDb);
                }

                await _context.SaveChangesAsync();

                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Result = ex.Message;

            }
            return _responseDto;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] ShoppingCartDto shoppingCartDto)
        {
            try
            {
                var header = await _context.CartHeaders.FirstAsync(c => c.Id == shoppingCartDto.CartHeader.Id);
                header.CouponCode = shoppingCartDto.CartHeader.CouponCode;
                _context.Update(header);
                _context.SaveChangesAsync();
                _responseDto.Result = true;
                _responseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }


    }
}
