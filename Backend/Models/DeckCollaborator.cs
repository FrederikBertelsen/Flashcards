using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models;

public class DeckCollaborator
{
    [MaxLength(Constants.MaxIdLength)] public required string DeckId { get; init; }
    public required Deck Deck { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string UserId { get; init; }
    public required User User { get; init; }
}