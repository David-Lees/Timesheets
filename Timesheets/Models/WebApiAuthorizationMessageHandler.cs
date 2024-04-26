using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace Timesheets.Models;

public class WebApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public WebApiAuthorizationMessageHandler(IConfiguration config, IAccessTokenProvider provider, NavigationManager navigation) : base(provider, navigation)
    {
        var scope = config.GetValue<string>("Scope") ?? string.Empty;
        var apiUrl = config.GetValue<string>("ApiUrl") ?? string.Empty;
        ConfigureHandler(authorizedUrls: [apiUrl], scopes: [scope]);
    }
}