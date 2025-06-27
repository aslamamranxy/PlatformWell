namespace PlatformWell.Services.AuthServices;

public class TokenHandler(TokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await tokenService.GetTokenAsync();
        
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Trim('"'));
        }
        
        var response = await base.SendAsync(request, cancellationToken);
        
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await tokenService.RefreshTokenAsync();
            
            token = await tokenService.GetTokenAsync();
            
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            response = await base.SendAsync(request, cancellationToken);    
        }
        
        return response;
    }
}