using Microsoft.AspNetCore.Hosting;
using ChatHub.Application.IRepository;
using ChatHub.Application;
using ChatHub.Infrastructures.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChatHub.Application.IService;
using ChatHub.Infrastructures.Service;


namespace ChatHub.Infrastructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructuresService(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            var connectionString = configuration.GetConnectionString("CONNECTION_STRING");
            services.AddDbContext<ChatDbContext>(
                option => option.UseSqlServer(connectionString));

            return services;
        } 
    }
}
