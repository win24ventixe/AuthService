namespace Presentation.Providers;

public class TokenProvider(IHttpContextAccessor contextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public Task<string> GetTokenAsync()
    {
        var token = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        return Task.FromResult(token ?? string.Empty);
    }
}
