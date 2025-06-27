using PlatformWell.Infra.Data;
using PlatformWell.Services.AuthServices;
using PlatformWell.Services.PlatformWellServices;

namespace PlatformWell.Services.DI;

public class ServiceManager
{
    public AuthService AuthService { get; }
    
    private readonly Lazy<PlatformWellService>  _lazyPlatformWellService;
    
    public ServiceManager(
        AppDbContext dbContext,
        IHttpClientFactory httpClientFactory,
        AuthService authService)
    {
        AuthService = authService;
        
        _lazyPlatformWellService = new Lazy<PlatformWellService>(() => new PlatformWellService(httpClientFactory, dbContext));
    }
    
    public PlatformWellService PlatformWellService => _lazyPlatformWellService.Value;
}