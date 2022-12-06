namespace ChantingApp.Persistence;

public class ChantStream
{
    public Guid ChantId { get; set; }
    public string? Url { get; set; }
    public StreamStatus Status { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
}

public enum StreamStatus
{
    Ready = 1,
    Live,
    Ended
}