using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models.DTOs;

public class UserDTO
{
    public required string Id { get; init; }
    public bool IsAdmin { get; init; } = false;
    public required string Name { get; init; }
    public required string GoogleId { get; init; }
    public required string PictureUrl { get; init; }

}