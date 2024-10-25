using System.ComponentModel.DataAnnotations;
using Backend.Utils;

namespace Backend.Models;

public class Session
{ 
    [MaxLength(Constants.MaxSessionIdLength)] public required string Id { get; init; }
    [MaxLength(Constants.MaxIdLength)] public required string UserId { get; init; }
    public required DateTime ExpiresAt { get; set; }
    public bool Fresh { get; set; } = true;
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}