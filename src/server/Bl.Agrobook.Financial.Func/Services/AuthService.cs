using Bl.Agrobook.Financial.Func.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bl.Agrobook.Financial.Func.Services;

public class AuthService
{
    private readonly IOptions<AuthOptions> _opt;

    public AuthService(IOptions<AuthOptions> opt)
    {
        _opt = opt;
    }

    public bool IsAuthenticated(HttpRequest req, string tokenKey = "x-api-key")
    {
        if (!req.Headers.TryGetValue(tokenKey, out var apiKey))
        {
            return false;
        }
        return IsAuthenticated(apiKey);
    }
    public bool IsAuthenticated(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var envToken = _opt.Value.Key;

        return token == envToken;
    }
}
