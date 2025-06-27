using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformWell.Application.Constant;
using PlatformWell.Core.Models.Options;
using PlatformWell.Infra.Data;
using PlatformWell.Services.DI;
using PlatformWell.Services.AuthServices;

namespace PlatformWell.Application.Services.Startup;

public static partial class ServiceInitializer
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiAuthOptions>(configuration.GetSection(ApiAuthOptions.Position));
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(ConnectionStringKeys.AppDb),
                sqlOptions => sqlOptions.MigrationsAssembly("PlatformWell.Infra"));
        });

        services.AddScoped<AuthService>();
        services.AddScoped<TokenService>();
        services.AddScoped<TokenHandler>();
        
        services.AddHttpClient("PlatformApi")
            .AddHttpMessageHandler<TokenHandler>();
        
        services.AddScoped<ServiceManager>();
    }
}