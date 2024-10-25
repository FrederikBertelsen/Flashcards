using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs;

public class DeckDTO
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required bool IsPublic { get; init; }
    public required UserDTO Creator { get; init; } 
    public required int FlashcardCount { get; init; }
}