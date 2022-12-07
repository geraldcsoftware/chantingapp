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

    public string? GetUserId()
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId)) return userId;

        // With no proper authentication rules as yet, return something to use in place of userid
        return _httpContext.Connection?.Id ?? Guid.NewGuid().ToString();
    }

    public bool IsAuthenticatedUser() => _httpContext.User.Identity?.IsAuthenticated ?? false;
}