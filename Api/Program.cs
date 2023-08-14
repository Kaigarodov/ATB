using Api.Helpers.Extensions;
using Dal;
using Logic;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDalServices(builder.Configuration);
builder.Services.AddLogicServices();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.LoginPath = "/account/login";
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper();

var app = builder.Build();

app.UpdateDatabase();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapAreaControllerRoute(
    name:"account",
    areaName:"Account",
    pattern:"{area}/{controller=Account}/{action=Index}");
app.MapAreaControllerRoute(
    name:"cabinet",
    areaName:"Cabinet",
    pattern:"{controller=Cabinet}/{action=Index}");

app.Run();
