namespace ChantingApp.Api.Hubs;

public interface IChantStream
{
    Task StreamStarted();
    Task UserJoined();
    Task UserLeft();
    Task StreamEnded(string name);
}