using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ShopBook.API.Infrastructures.Extensions
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Thông tin mô tả API
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BookStore API",
                    Version = "v1",
                    Description = "Web API cho hệ thống thương mại điện tử",
                    Contact = new OpenApiContact
                    {
                        Name = "Duc Loc",
                        Email = "tranducloc2010@gmail.com"
                    }
                });

                // Đọc XML comment
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                // Cấu hình JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token theo định dạng: Bearer {token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "BookStore API Docs";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API V1");
            });

            return app;
        }
    }
}
