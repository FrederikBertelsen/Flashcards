using Backend.Exceptions;
using Backend.Models;
using Backend.Models.DTOs;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/decks/{deckId}/collaborators")]
[ApiController]
public class CollaboratorController(CollaboratorRepository collaboratorRepository, SessionManager sessionManager)
    : BaseController(sessionManager)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CollaboratorDTO>>> GetDeckCollaborators(string deckId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            var collaboratorDtos = await collaboratorRepository.GetDeckCollaborators(deckId, user);
            return Ok(collaboratorDtos);
        });
    }

    [HttpPut]
    public async Task<ActionResult> UpdateDeckCollaborators(string deckId, string[] collaboratorIds)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await collaboratorRepository.UpdateDeckCollaborators(deckId, collaboratorIds, user);
                return Ok();
            });
        });
    }
}