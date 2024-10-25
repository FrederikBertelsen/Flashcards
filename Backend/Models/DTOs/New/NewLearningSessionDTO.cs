using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs.New;

public class NewLearningSessionDTO
{
    [MaxLength(Constants.MaxNameLength)] public required string Name { get; init; } 

    [MaxLength(Constants.MaxIdLength)] public required string UserId { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string DeckId { get; init; }
    
    public required ICollection<NewReviewDto> Reviews { get; init; }
}