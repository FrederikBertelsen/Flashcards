using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models;

public class Flashcard
{
    [MaxLength(Constants.MaxIdLength)] public string Id { get; init; } = UniqueIdGenerator.Generate();
    public required FlashType FlashType { get; set; }
    [MaxLength(Constants.MaxTextLength)] public required string Front { get; set; }
    [MaxLength(Constants.MaxTextLength)] public required string? Back { get; set; }

    [MaxLength(Constants.MaxIdLength)] public required string DeckId { get; init; }
    public required Deck Deck { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.Now;
}

public enum FlashType
{
    Normal = 0,
    Cloze = 1
}