using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs;

public class ReviewDTO
{
    [MaxLength(Constants.MaxIdLength)] public required string Id { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string LearningSessionId { get; init; }

    public required int ClozeIndex { get; init; }

    public required Card Card { get; init; }
}