using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PlatformWell.Core.Entities;
using PlatformWell.Core.DTOs;
using PlatformWell.Core.Interfaces.Core;
using PlatformWell.Infra.Data;
using PlatformWell.Services.Constants.Api;

namespace PlatformWell.Services.PlatformWellServices;

public class PlatformWellService(IHttpClientFactory httpClientFactory, AppDbContext dbContext)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("PlatformApi");
    public async Task<IEnumerable<PlatformDto>> GetActualPlatformWellAsync()
    {
        var response = await _httpClient.GetAsync(ApiBaseUrl.Url + "/PlatformWell/GetPlatFormWellActual");
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Failed to fetch platform: " + response.StatusCode);
            return null;
        }
        
        var platform = await response.Content.ReadFromJsonAsync<IEnumerable<PlatformDto>>();
        
        Console.WriteLine("Platform: " + JsonSerializer.Serialize(platform, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        
        return platform;
    }
    
    public async Task<IEnumerable<DummyPlatformDto>> GetDummyPlatformWellAsync()
    {
        var response = await _httpClient.GetAsync(ApiBaseUrl.Url + "/PlatformWell/GetPlatformWellDummy");
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Failed to fetch platform: " + response.StatusCode);
            return null;
        }
        
        var platform = await response.Content.ReadFromJsonAsync<IEnumerable<DummyPlatformDto>>();
        
        Console.WriteLine("Platform: " + JsonSerializer.Serialize(platform, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        
        return platform;
    }

    public async Task SaveActualPlatformWellAsync(IEnumerable<PlatformDto> platforms)
    {
        try
        {
           foreach (var platformData in platforms)
           {
                var existingPlatform = await dbContext.Platforms
                    .Include(p => p.Wells)
                    .FirstOrDefaultAsync(p => p.Id == platformData.Id);

                if (existingPlatform == null)
                {
                    var newPlatform = new Platform
                    {
                        Id = platformData.Id,
                        UniqueName = platformData.UniqueName,
                        Latitude = platformData.Latitude,
                        Longitude = platformData.Longitude,
                        CreatedAt = platformData.CreatedAt,
                        UpdatedAt = platformData.UpdatedAt,
                        Wells = new List<Well>()
                    };
        
                    if (platformData.Well?.Count > 0)
                    {
                        foreach (var wellData in platformData.Well)
                        {
                            var newWell = new Well
                            {
                                Id = wellData.Id,
                                PlatformId = wellData.PlatformId,
                                UniqueName = wellData.UniqueName,
                                Latitude = wellData.Latitude,
                                Longitude = wellData.Longitude,
                                CreatedAt = wellData.CreatedAt,
                                UpdatedAt = wellData.UpdatedAt
                            };
                            
                            newPlatform.Wells.Add(newWell);
                        }
                    }
                    
                    dbContext.Platforms.Add(newPlatform);
                }
                else
                {
                    existingPlatform.UniqueName = platformData.UniqueName;
                    existingPlatform.Latitude = platformData.Latitude;
                    existingPlatform.Longitude = platformData.Longitude;
                    existingPlatform.CreatedAt = platformData.CreatedAt;
                    existingPlatform.UpdatedAt = platformData.UpdatedAt;

                    if (platformData.Well?.Count > 0)
                    {
                        foreach (var wellData in platformData.Well)
                        {
                            var existingWell = existingPlatform.Wells?.FirstOrDefault(w => w.Id == wellData.Id);

                            if (existingWell == null)
                            {
                                var newWell = new Well
                                {
                                    Id = wellData.Id,
                                    PlatformId = wellData.PlatformId,
                                    UniqueName = wellData.UniqueName,
                                    Latitude = wellData.Latitude,
                                    Longitude = wellData.Longitude,
                                    CreatedAt = wellData.CreatedAt,
                                    UpdatedAt = wellData.UpdatedAt
                                };
                                
                                existingPlatform.Wells?.Add(newWell);
                            }
                            else
                            {
                                existingWell.PlatformId = platformData.Id;
                                existingWell.UniqueName = wellData.UniqueName;
                                existingWell.Latitude = wellData.Latitude;
                                existingWell.Longitude = wellData.Longitude;
                                existingWell.CreatedAt = wellData.CreatedAt;
                                existingWell.UpdatedAt = wellData.UpdatedAt;
                            }
                        }
                    }
                }
           }
           
           dbContext.ChangeTracker.Entries<IAuditableEntity>().ToList().ForEach(entry =>
           {
               if (entry.State == EntityState.Added)
               {
                   entry.Property(x => x.CreatedAt).IsModified = false;
                   entry.Property(x => x.UpdatedAt).IsModified = false;
               }
           });
           
           await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task SaveDummyPlatformWellAsync(IEnumerable<DummyPlatformDto> platforms)
    {
        try
        {
           foreach (var platformData in platforms)
           {
                var existingPlatform = await dbContext.Platforms
                    .Include(p => p.Wells)
                    .FirstOrDefaultAsync(p => p.Id == platformData.Id);

                if (existingPlatform == null)
                {
                    var newPlatform = new Platform
                    {
                        Id = platformData.Id,
                        UniqueName = platformData.UniqueName,
                        Latitude = platformData.Latitude,
                        Longitude = platformData.Longitude,
                        LastUpdate = platformData.LastUpdate,
                        Wells = new List<Well>()
                    };
        
                    if (platformData.Well?.Count > 0)
                    {
                        foreach (var wellData in platformData.Well)
                        {
                            newPlatform.Wells.Add(new Well
                            {
                                Id = wellData.Id,
                                PlatformId = wellData.PlatformId,
                                UniqueName = wellData.UniqueName,
                                Latitude = wellData.Latitude,
                                LastUpdate = wellData.LastUpdate,
                                Longitude = wellData.Longitude,
                            });
                        }
                    }
                    
                    dbContext.Platforms.Add(newPlatform);
                }
                else
                {
                    existingPlatform.UniqueName = platformData.UniqueName;
                    existingPlatform.Latitude = platformData.Latitude;
                    existingPlatform.Longitude = platformData.Longitude;
                    existingPlatform.LastUpdate = platformData.LastUpdate;

                    if (platformData.Well?.Count > 0)
                    {
                        foreach (var wellData in platformData.Well)
                        {
                            var existingWell = existingPlatform.Wells?.FirstOrDefault(w => w.Id == wellData.Id);

                            if (existingWell == null)
                            {
                                existingPlatform.Wells.Add(new Well
                                {
                                    Id = wellData.Id,
                                    PlatformId = wellData.PlatformId,
                                    UniqueName = wellData.UniqueName,
                                    Latitude = wellData.Latitude,
                                    Longitude = wellData.Longitude,
                                    LastUpdate = wellData.LastUpdate,
                                });
                            }
                            else
                            {
                                existingWell.PlatformId = platformData.Id;
                                existingWell.UniqueName = wellData.UniqueName;
                                existingWell.Latitude = wellData.Latitude;
                                existingWell.Longitude = wellData.Longitude;
                                existingWell.LastUpdate = wellData.LastUpdate;
                            }
                        }
                    }
                }
           }
           
           await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
 
    }
}