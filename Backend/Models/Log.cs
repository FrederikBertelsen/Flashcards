using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

[Owned]
public class Log
{
    public required Rating Rating { get; init; }
    public required State State { get; init; }
    public required DateTime Due { get; init; }
    public required float Stability { get; init; }
    public required float Difficulty { get; init; }
    public required int ElapsedDays { get; init; }
    public required int LastElapsedDays { get; init; }
    public required int ScheduledDays { get; init; }
    public required DateTime Review { get; init; }
}