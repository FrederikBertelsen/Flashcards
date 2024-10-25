using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs;

public class FlashcardDTO
{
    public required string Id { get; init; }
    public required FlashType FlashType { get; init; }
    public required string Front { get; init; }
    public required string Back { get; init; }
}