using System.Security.Claims;
using System.Text.Encodings.Web;
using CloudBlue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace CLoudBlue.Apis.AccountAuthentications;

public class ApiKeyAuthenticationHandler(
    IUsersSessionService usersSessionService,
    IOptionsMonitor<ApiKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if(!Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderName, out var apiKey) || apiKey.Count != 1)
        {
            return AuthenticateResult.Fail("Invalid or missing apiX");
        }

        var userInfo = await usersSessionService.GetUserSessionAsync(apiKey.ToString());

        if(userInfo == null)
        {
            return AuthenticateResult.Fail("Invalid credentials");
        }

        var claims = new[]
        {
            new Claim("UserId", userInfo.UserId.ToString())

            //    new Claim("MainAccountId", userInfo.MainAccountId.ToString())
        };

        var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.DefaultScheme);
        var identities = new List<ClaimsIdentity> { identity };
        var principal = new ClaimsPrincipal(identities);
        var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.DefaultScheme);

        return AuthenticateResult.Success(ticket);
    }
}