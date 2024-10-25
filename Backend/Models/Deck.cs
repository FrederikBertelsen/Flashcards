using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models;

public class Deck
{
    [MaxLength(Constants.MaxIdLength)] public string Id { get; init; } = UniqueIdGenerator.Generate();
    [MaxLength(Constants.MaxNameLength)] public required string Name { get; set; }
    public bool IsPublic { get; set; } = true;
    
    [MaxLength(Constants.MaxIdLength)] public required string UserId { get; init; }
    public required User User { get; init; }
    
    public ICollection<Flashcard> Flashcards { get; init; } = [];
    
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}