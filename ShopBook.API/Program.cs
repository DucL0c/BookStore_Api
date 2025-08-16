using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopBook.API.Infrastructure.Extentsions;
using ShopBook.API.Infrastructures.Extensions;
using ShopBook.Data;
using ShopBook.Data.Repositories;
using ShopBook.Service;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// 1. Đăng ký DbContext với DI container mặc định
builder.Services.AddDbContext<BookstoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<BookstoreContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure())
//);

// 2. Cấu hình Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Đăng ký module Autofac
    containerBuilder.RegisterModule(new ContainerModule());

    // Cấu hình AutoMapper
    containerBuilder.RegisterInstance(AutoMapperConfig.Initialize()).SingleInstance();

    // Đảm bảo cả Repository và Service đều được đăng ký
    containerBuilder.RegisterAssemblyTypes(typeof(UsersRepository).Assembly)
          .Where(t => t.Name.EndsWith("Repository"))
          .AsImplementedInterfaces()
          .InstancePerLifetimeScope();

    containerBuilder.RegisterAssemblyTypes(typeof(UsersService).Assembly)
          .Where(t => t.Name.EndsWith("Service"))
          .AsImplementedInterfaces()
          .InstancePerLifetimeScope();
});

// Đảm bảo AddAutofac() được gọi trước khi build
builder.Services.AddAutofac();
// Add services to the container.

// Jwt authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// Cors configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseCors("AllowAngularClient");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
