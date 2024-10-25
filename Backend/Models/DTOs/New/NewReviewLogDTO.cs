using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs;

public class NewReviewLogDTO
{
    [MaxLength(Constants.MaxIdLength)] public required string UserId { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string LearningSessionId { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string ReviewId { get; init; }
    
    public required Log Log { get; init; }
}