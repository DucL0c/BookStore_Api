using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopBook.API.Infrastructure.Extentsions;
using ShopBook.Data;
using ShopBook.Data.Repositories;
using ShopBook.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// 1. Đăng ký DbContext với DI container mặc định
builder.Services.AddDbContext<BookstoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
