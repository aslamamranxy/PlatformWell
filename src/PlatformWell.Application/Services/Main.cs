using Microsoft.Extensions.Options;
using PlatformWell.Core.Models.Options;
using PlatformWell.Services.DI;

namespace PlatformWell.Application.Services;

public class Main(ServiceManager serviceManager)
{
    public async Task RunAsync()
    {
        var actualPlatforms = await serviceManager.PlatformWellService.GetActualPlatformWellAsync();
        
        await serviceManager.PlatformWellService.SaveActualPlatformWellAsync(actualPlatforms);
        
        var dummyPlatforms = await serviceManager.PlatformWellService.GetDummyPlatformWellAsync();
        
        await serviceManager.PlatformWellService.SaveDummyPlatformWellAsync(dummyPlatforms);
    }
}