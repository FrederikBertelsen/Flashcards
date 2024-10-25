using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models;

public class LearningSession
{
    [MaxLength(Constants.MaxIdLength)] public string Id { get; init; } = UniqueIdGenerator.Generate();
    [MaxLength(Constants.MaxNameLength)] public required string Name { get; init; } 

    [MaxLength(Constants.MaxIdLength)] public required string UserId { get; init; }
    public required User User { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string DeckId { get; init; }
    public required Deck Deck { get; init; }

    public ICollection<Review> Reviews { get; init; } = [];
    public ICollection<ReviewLog> ReviewLogs { get; init; } = [];
    
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}