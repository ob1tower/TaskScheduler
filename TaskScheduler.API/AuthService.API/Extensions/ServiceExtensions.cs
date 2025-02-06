using AuthService.API.DataAccess;
using AuthService.API.Repositories;
using AuthService.API.Repositories.Interfaces;
using AuthService.API.Services;
using AuthService.API.Services.Interfaces;
using AuthService.API.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<ITokensService, TokensService>();

        services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("UserDbContext")));

        services.AddSingleton<IRabbitMqService>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMqService>>();
            var hostName = "localhost";
            return new RabbitMqService(hostName, logger);
        });

        return services;
    }
}
