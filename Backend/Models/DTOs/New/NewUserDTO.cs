using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DTOs.New;

public class NewUserDTO
{
    public required string Name { get; init; }
    public required string GoogleId { get; init; }
    public required string PictureUrl { get; init; }
}