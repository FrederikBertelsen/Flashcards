using Backend.Exceptions;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/decks")]
[ApiController]
public class DecksController(DecksRepository decksRepository, SessionManager sessionManager)
    : BaseController(sessionManager)
{
    [HttpPost]
    public async Task<ActionResult<DeckDTO>> CreateDeck(NewDeckDTO newDeckDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                var deckDto = await decksRepository.CreateDeck(newDeckDto, user);
                return Ok(deckDto);
            });
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeckDTO>>> GetDecks()
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            var deckDtos = await decksRepository.GetDecks(user);
            return Ok(deckDtos);
        });
    }

    [HttpGet("{deckId}")]
    public async Task<ActionResult<DeckDTO>> GetDeckById(string deckId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            var deckDto = await decksRepository.GetDeckById(deckId, user);
            return Ok(deckDto);
        });
    }
    
    [HttpGet("/api/users/{userId}/decks")]
    public async Task<ActionResult<IEnumerable<DeckDTO>>> GetDecksByUserId(string userId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            var deckDtos = await decksRepository.GetDecksByUserId(userId, user);
            return Ok(deckDtos);
        });
    }

    [HttpGet("search/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<DeckDTO>>> GetDecksBySearchTerm(string searchTerm)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            var matchedDeckDtos = await decksRepository.GetDecksBySearchTerm(searchTerm, user);
            return Ok(matchedDeckDtos);
        });
    }

    [HttpPut("{deckId}")]
    public async Task<ActionResult> UpdateDeck(UpdateDeckDTO updateDeckDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await decksRepository.UpdateDeck(updateDeckDto, user);
                return NoContent();
            });
        });
    }

    [HttpDelete("{deckId}")]
    public async Task<IActionResult> DeleteDeck(string deckId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await decksRepository.DeleteDeck(deckId, user);
                return NoContent();
            });
        });
    }
}