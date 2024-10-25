using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models;

public class ReviewLog
{
    [MaxLength(Constants.MaxIdLength)] public string Id { get; init; } = UniqueIdGenerator.Generate();
    
    [MaxLength(Constants.MaxIdLength)] public required string LearningSessionId { get; init; }
    public required LearningSession LearningSession { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string ReviewId { get; init; }
    public required Review Review { get; init; }

    public required Log Log { get; init; }
}

public enum Rating
{
    Manual = 0,
    Again = 1,
    Hard = 2,
    Good = 3,
    Easy = 4
}