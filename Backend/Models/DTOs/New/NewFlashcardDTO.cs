namespace Backend.Models.DTOs.New;

public class NewFlashcardDTO
{
    public required string DeckId { get; init; }
    public required FlashType FlashType { get; init; }
    public required string Front { get; init; }
    public required string Back { get; init; }
}