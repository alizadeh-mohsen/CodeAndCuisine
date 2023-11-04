using CodeAndCuisine.Web.Services;
using CodeAndCuisine.Web.Services.IService;
using CodeAndCuisine.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); //neede when working with cookies
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();

StaticData.CouponApiBase = builder.Configuration["ServiceUrls:CouponApi"];
StaticData.AuthApiBase = builder.Configuration["ServiceUrls:AuthApi"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
