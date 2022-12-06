using System.Security.Claims;

namespace ChantingApp.Api.Services;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly HttpContext _httpContext;

    public CurrentUserContext(IHttpContextAccessor contextAccessor)
    {
        _httpContext = contextAccessor.HttpContext ??
                       throw new InvalidOperationException("Cannot initialize this class outside an HttpContext session");
    }

    public string? GetUserId() => _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public bool IsAuthenticatedUser() => _httpContext.User.Identity?.IsAuthenticated ?? false;
}