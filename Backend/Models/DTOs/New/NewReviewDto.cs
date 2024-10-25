using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs.New;

public class NewReviewDto
{
    [MaxLength(Constants.MaxIdLength)] public required string LearningSessionId { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string FlashcardId { get; init; }

    public required int ClozeIndex { get; init; }

    public required Card Card { get; init; }
}