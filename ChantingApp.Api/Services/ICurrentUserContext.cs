namespace ChantingApp.Api.Services;

public interface ICurrentUserContext
{
    string? GetUserId();
    bool IsAuthenticatedUser();
}