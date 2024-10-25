using Backend.Exceptions;
using Backend.Models.DTOs;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/import")]
[ApiController]
public class ImportController(ImportRepository importRepository, SessionManager sessionManager)
    : BaseController(sessionManager)
{
    [HttpPost("quizlet/{quizletDeckId}")]
    public async Task<ActionResult<DeckDTO>> ImportQuizletDeck(string quizletDeckId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                var deckDto = await importRepository.ImportQuizletDeck(quizletDeckId, user);
                return Ok(deckDto);
            });
        });
    }
    
}