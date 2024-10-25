namespace Backend.Models.DTOs.New;

public class NewSessionDTO
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required DateTime ExpiresAt { get; init; }
}