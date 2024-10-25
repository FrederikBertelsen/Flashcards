namespace Backend.Models.DTOs;

public class UpdateDeckDTO
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required bool IsPublic { get; init; }
}