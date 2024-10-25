using Backend.Models;
using Backend.Models.DbContext;

namespace Backend.Utils;

public class SessionManager(AppDbContext dbContext)
{
    public async Task<User?> ValidateRequestAsync(HttpRequest request)
    {
        string? sessionId = GetSessionIdFromRequest(request);
        
        Session? session = await ValidateSessionAsync(sessionId);
        if (session == null)
            return null;
        
        return await dbContext.Users.FindAsync(session.UserId);
    }
    
    private async Task<Session?> ValidateSessionAsync(string? sessionId)
    {
        if (string.IsNullOrEmpty(sessionId) || sessionId.Length > Constants.MaxSessionIdLength)
            return null;

        var session = await dbContext.Sessions.FindAsync(sessionId);
        if (session == null || session.ExpiresAt < DateTime.UtcNow)
            return null;

        return session;
    }
    
    private string? GetSessionIdFromRequest(HttpRequest request)
    {
        var authHeader = request.Headers.Authorization.FirstOrDefault();
        
        if (authHeader == null || !authHeader.StartsWith("Bearer ")) 
            return null;
        
        var sessionId = authHeader["Bearer ".Length..].Trim();
        
        return !string.IsNullOrEmpty(sessionId) ? sessionId : null;

    }



    private async Task<User?> GetUserFromSessionIdAsync(string? sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
            return null;
        
        Session? session = await ValidateSessionAsync(sessionId);
        if (session == null)
            return null;
        
        return await dbContext.Users.FindAsync(session.UserId);
    }
}