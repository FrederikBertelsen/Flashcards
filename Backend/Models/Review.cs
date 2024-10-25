using System.ComponentModel.DataAnnotations;
using Backend.Models.DTOs;
using Backend.Utils;

namespace Backend.Models;

public class Review
{
    [MaxLength(Constants.MaxIdLength)] public string Id { get; init; } = UniqueIdGenerator.Generate();
    
    [MaxLength(Constants.MaxIdLength)] public required string LearningSessionId { get; init; }
    public required LearningSession LearningSession { get; init; }
    
    [MaxLength(Constants.MaxIdLength)] public required string FlashcardId { get; init; }
    public required Flashcard Flashcard { get; init; }
    public required int ClozeIndex { get; init; } = 0;

    public required Card Card { get; set; }
}

public enum State
{
    New = 0,
    Learning = 1,
    Review = 2,
    Relearning = 3
}