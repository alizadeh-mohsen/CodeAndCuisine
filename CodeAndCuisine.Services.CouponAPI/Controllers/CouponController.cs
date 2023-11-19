using AutoMapper;
using CodeAndCuisine.Services.CouponAPI.Data;
using CodeAndCuisine.Services.CouponAPI.Models;
using CodeAndCuisine.Services.CouponAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeAndCuisine.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly CodeDbContext _codeDbContext;
        private readonly IMapper _mapper;
        ResponseDto _responseDto;
        public CouponController(CodeDbContext codeDbContext, IMapper mapper)
        {
            _codeDbContext = codeDbContext;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                var coupons = _codeDbContext.Coupons.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = CreateErrorMessage(ex);
            }
            return _responseDto;

        }

        [HttpGet("{id}")]
        public ResponseDto Get(int id)
        {
            try
            {
                var coupon = _codeDbContext.Coupons.Find(id);
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
                if (coupon == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No coupuns found!";
                }
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = CreateErrorMessage(ex);
            }

            return _responseDto;
        }


        [HttpGet("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {

            try
            {

                var coupon = _codeDbContext.Coupons.FirstOrDefault(c => c.CouponCode == code);
                if (coupon == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No coupuns found!";
                }
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = CreateErrorMessage(ex);
            }

            return _responseDto;

        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDto Post([FromBody] Coupon couponDto)
        {

            try
            {

                var coupon = _mapper.Map<Coupon>(couponDto);

                _codeDbContext.Coupons.Add(coupon);
                _codeDbContext.SaveChanges();

                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
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

        public ResponseDto Put([FromBody] Coupon couponDto)
        {

            try
            {

                var coupon = _mapper.Map<Coupon>(couponDto);

                _codeDbContext.Coupons.Update(coupon);
                _codeDbContext.SaveChanges();

                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
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
        public ResponseDto Delete(int id)
        {
            try
            {

                var coupon = _codeDbContext.Coupons.Find(id);

                _codeDbContext.Coupons.Remove(coupon);
                _codeDbContext.SaveChanges();

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