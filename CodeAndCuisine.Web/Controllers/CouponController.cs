using CodeAndCuisine.Web.Models;
using CodeAndCuisine.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace CodeAndCuisine.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<CouponDto> coupons = new();
            var response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            else
                TempData["error"] = response.Message;

            return View(coupons);
        }

        #region Create


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                model.CouponCode = model.CouponCode.ToUpper();
                var responseDto = await _couponService.CreateCouponAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction("Index");
                }

                else
                    TempData["error"] = responseDto.Message;
            }

            return View(model);


        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {

            var couponDto = await GetCoupon(id);

            if (couponDto != null)

                return View(couponDto);


            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                model.CouponCode = model.CouponCode.ToUpper();
                var responseDto = await _couponService.UpdateCouponAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Coupon updated successfully";
                    return RedirectToAction("Index");
                }

                else
                    TempData["error"] = responseDto.Message;
            }


            return View(model);


        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {

            var couponDto = await GetCoupon(id);

            if (couponDto != null)

                return View(couponDto);


            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(CouponDto couponDto)
        {
            var responseDto = await _couponService.DeleteCouponCouponAsync(couponDto.CouponId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction("Index");
            }

            else
                TempData["error"] = responseDto.Message;

            return View();
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {

            var couponDto = await GetCoupon(id);

            if (couponDto != null)

                return View(couponDto);


            return NotFound();
        }

        #endregion


        private async Task<CouponDto> GetCoupon(int couponId)
        {
            CouponDto coupon = null;

            var responseDto = await _couponService.GetCouponByIdAsync(couponId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }
            return coupon;
        }
    }
}