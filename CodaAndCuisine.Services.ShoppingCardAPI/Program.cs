using AutoMapper;
using CodaAndCuisine.Services.ShoppingCartAPI.Service;
using CodaAndCuisine.Services.ShoppingCartAPI.Service.IService;
using CodeAndCuisine.Services.ShoppingCartAPI.Data;
using CodeAndCuisine.Services.ShoppingCartAPI.Extensions;
using CodeAndCuisine.Services.ShoppingCartAPI.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Add services to the container.
builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(
    builder.Configuration["ServiceUrls:ProductApi"]
    ));

builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(
    builder.Configuration["ServiceUrls:CouponApi"]
    ));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();    

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
