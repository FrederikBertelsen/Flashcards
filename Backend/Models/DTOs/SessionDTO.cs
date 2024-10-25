using Backend.Utils;

namespace Backend.Models.DTOs;

public class SessionDTO
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required DateTime ExpiresAt { get; set; }
    public required bool Fresh { get; set; }
}