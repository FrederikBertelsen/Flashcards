using Microsoft.EntityFrameworkCore;

namespace Backend.Models.DTOs;

[Owned]
public class Card
{
    public required DateTime Due { get; set; }
    public required float Stability { get; set; }
    public required float Difficulty { get; set; }
    public required int ElapsedDays { get; set; }
    public required int ScheduledDays { get; set; }
    public required int Reps { get; set; }
    public required int Lapses { get; set; }
    public required State State { get; set; }
    public DateTime? LastReview { get; set; }
}