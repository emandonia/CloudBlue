using Microsoft.AspNetCore.Authentication;

namespace CLoudBlue.Apis.AccountAuthentications;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "MimoApiKey";
    public const string HeaderName = "x-api-key";
}