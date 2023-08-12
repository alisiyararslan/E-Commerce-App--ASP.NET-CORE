using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using DataAccessLayer.Contexts;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using WebApiTest.Extensions;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.ConfigureSqlContext(builder.Configuration);

//User dependency injection
builder.Services.ConfigureUserManager();
builder.Services.ConfigureGenericDal();

//Address dependency injection
builder.Services.ConfigureAddressManager();
builder.Services.ConfigureAddressDal();

//Category dependency injection
builder.Services.ConfigureCategoryManager();
builder.Services.ConfigureCategoryDal();

//SubCategory dependency injection
builder.Services.ConfigureSubCategoryManager();
builder.Services.ConfigureSubCategoryDal();

//CategoryDetail dependency injection
builder.Services.ConfigureCategoryDetailManager();
builder.Services.ConfigureCategoryDetailDal();

//Item dependency injection
builder.Services.ConfigureItemManager();
builder.Services.ConfigureItemDal();

//Order dependency injection
builder.Services.ConfigureOrderManager();
builder.Services.ConfigureOrderDal();

//OrderDetail dependency injection
builder.Services.ConfigureOrderDetailManager();
builder.Services.ConfigureOrderDetailDal();

//Reminder dependency injection
builder.Services.ConfigureReminderManager();
builder.Services.ConfigureReminderDal();

//FavoriteItemUser Dependency Injection
builder.Services.ConfigureFavoriteItemUserManager();
builder.Services.ConfigureFavoriteItemUserDal();


//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//})
//.AddCookie(options =>
//{
//    options.Cookie.Name = "ShopListApp";
//    options.LoginPath = "/Session/Login"; // Giriþ sayfasýnýn yolunu belirtin
//    options.AccessDeniedPath = "/Session/Login"; // Yetkisiz eriþimde yönlendirilecek yol
//});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // Configure your JWT options here
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Add other token validation parameters here
    };
});

// Autofac container oluþturma


//containerBuilder.RegisterType<UserManager>().As<IUserService>().SingleInstance();
//containerBuilder.RegisterType<EfUserDal>().As<IUserDal>().SingleInstance();


// var containerBuilder = new ContainerBuilder();
//containerBuilder.Populate(builder.Services); // Microsoft DI'dan Autofac'a geçiþ

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer(); 
builder.Logging.ClearProviders();

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>//CORS 
{
    build.WithOrigins("https://localhost:7264").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");//CORS

app.UseHttpsRedirection();

app.UseCors(builder => builder.WithOrigins("http://localhost:7297").AllowAnyHeader());

app.UseAuthentication(); // UseAuthentication metodu düzenlendi
app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    // Endpoint yapýlandýrmalarýnýzý burada tanýmlayýn
//    endpoints.MapControllers();
//});
//app.UseRouting();//chat gpt ekledi

app.MapControllers();

app.Run();