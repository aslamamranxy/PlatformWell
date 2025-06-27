namespace PlatformWell.Services.AuthServices;

public class TokenService(AuthService authService)
{
    private string? _token;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public async Task<string?> GetTokenAsync()
    {
        if (string.IsNullOrEmpty(_token))
        {
            await RefreshTokenAsync();
        }

        return _token;
    }
    
    public async Task RefreshTokenAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            _token = await authService.LoginAsync();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}