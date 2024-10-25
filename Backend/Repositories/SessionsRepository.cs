using Backend.Exceptions;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class SessionsRepository(AppDbContext dbContext)
{
    public async Task<SessionDTO> CreateSession(NewSessionDTO newSessionDto)
    {
        var user = await dbContext.Users.FindAsync(newSessionDto.UserId);
        if (user == null)
            throw new ModelNotFoundException($"User with id {newSessionDto.UserId} not found.");

        var session = (await dbContext.Sessions.AddAsync(newSessionDto.ToEntity(user))).Entity;
        await dbContext.SaveChangesAsync();

        return session.ToDto();
    }

    public async Task<SessionDTO> GetSessionBySessionId(string sessionId)
    {
        var session = await dbContext.Sessions
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session == null)
            throw new ModelNotFoundException($"Session with SessionId {sessionId} not found");

        return session.ToDto();
    }

    public async Task<IEnumerable<Session>> GetUserSessions(string userId)
    {
        if (await dbContext.Users.FindAsync(userId) == null)
            throw new ModelNotFoundException($"User with id {userId} not found.");

        return await dbContext.Sessions
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateSessionExpiration(string sessionId, DateTime newExpiration)
    {
        var session = await dbContext.Sessions.FindAsync(sessionId);
        if (session == null)
            throw new ModelNotFoundException($"Session with id {sessionId} not found");

        session.ExpiresAt = newExpiration;
        dbContext.Sessions.Update(session);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteExpiredSessions()
    {
        var expiredSessions = dbContext.Sessions.Where(s => s.ExpiresAt <= DateTime.UtcNow);
        dbContext.Sessions.RemoveRange(expiredSessions);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteSession(string sessionId)
    {
        var session = await dbContext.Sessions.FindAsync(sessionId);
        if (session == null)
            throw new ModelNotFoundException($"Session with id {sessionId} not found");

        dbContext.Sessions.Remove(session);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserSessions(string userId)
    {
        if (await dbContext.Users.FindAsync(userId) == null)
            throw new ModelNotFoundException($"User with id {userId} not found.");

        var userSessions = dbContext.Sessions.Where(s => s.UserId == userId);
        dbContext.Sessions.RemoveRange(userSessions);
        await dbContext.SaveChangesAsync();
    }
}