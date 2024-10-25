using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models;

public class User
{
    [MaxLength(Constants.MaxIdLength)] public string Id { get; init; } = UniqueIdGenerator.Generate();
    public bool IsAdmin { get; init; } = false;
    [MaxLength(Constants.MaxNameLength)] public required string Name { get; set; }
    [MaxLength(Constants.MaxGoogleIdLength)] public required string GoogleId { get; init; }
    [MaxLength(Constants.MaxPictureUrlLength)] public required string PictureUrl { get; init; }
    
    public ICollection<Deck> Decks { get; init; } = [];
    public ICollection<LearningSession> LearningSessions { get; init; } = [];

    public DateTime CreatedAt { get; init; } = DateTime.Now;
}