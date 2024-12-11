using ChatHub.Global.Shared.JsonConverters;
using ChatHub.WebAPI.Conventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ChatHub.WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Conventions.Add(new ControllerNameAttributeConvention());
                    //options.ModelBinderProviders.Insert(0, new DateOnlyMoldelBinderProvider());
                })
                .ConfigureApiBehaviorOptions(option =>
                {
                    option.SuppressModelStateInvalidFilter = true;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 1024 * 1024;
            });


            services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
                c.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date-time",
                    //Pattern = DateTimeFormat.ToShortDate(Constant.DATE_TIME_FORMAT_MMddyyyy)
                });
            });
            services.AddHttpClient();
            return services;
        }
    }
}
