using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs;

public class LearningSessionDTO
{
    [MaxLength(Constants.MaxIdLength)] public required string Id { get; init; } = UniqueIdGenerator.Generate();
    [MaxLength(Constants.MaxNameLength)] public required string Name { get; init; }

    [MaxLength(Constants.MaxIdLength)] public required string DeckId { get; init; }

    public required int ReviewCount { get; init; }
    public required int DueReviewCount { get; init; }
}