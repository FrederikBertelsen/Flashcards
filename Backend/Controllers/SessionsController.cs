using Backend.Exceptions;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/sessions")]
[ApiController]
public class SessionsController(SessionsRepository sessionsRepository, SessionManager sessionManager)
    : BaseController(sessionManager)
{
    [HttpPost]
    public async Task<ActionResult<SessionDTO>> CreateSession(NewSessionDTO newSessionDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var sessionDto = await sessionsRepository.CreateSession(newSessionDto);
            return Ok(sessionDto);
        });
    }

    [HttpGet("{sessionId}")]
    public async Task<ActionResult<SessionDTO>> GetSessionBySessionId(string sessionId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var session = await sessionsRepository.GetSessionBySessionId(sessionId);
            return Ok(session);
        });
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<SessionDTO>>> GetUserSessions(string userId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var sessions = await sessionsRepository.GetUserSessions(userId);
            return Ok(sessions);
        });
    }

    [HttpPut("{sessionId}")]
    public async Task<ActionResult> UpdateSessionExpiration(string sessionId, DateTime newExpiration)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            await sessionsRepository.UpdateSessionExpiration(sessionId, newExpiration);
            return NoContent();
        });
    }

    [HttpDelete("expired")]
    public async Task<ActionResult> DeleteExpiredSessions()
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            await sessionsRepository.DeleteExpiredSessions();
            return NoContent();
        });
    }

    [HttpDelete("{sessionId}")]
    public async Task<ActionResult> DeleteSession(string sessionId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            await sessionsRepository.DeleteSession(sessionId);
            return NoContent();
        });
    }

    [HttpDelete("user/{userId}")]
    public async Task<ActionResult> DeleteUserSessions(string userId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            await sessionsRepository.DeleteUserSessions(userId);
            return NoContent();
        });
    }
}