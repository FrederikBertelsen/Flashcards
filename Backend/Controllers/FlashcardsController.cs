using Backend.Exceptions;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/flashcards")]
[ApiController]
public class FlashcardsController(
    FlashcardsRepository flashcardsRepository,
    SessionManager sessionManager)
    : BaseController(sessionManager)
{
    [HttpPost]
    public async Task<ActionResult<FlashcardDTO>> CreateFlashcard(NewFlashcardDTO newFlashcardDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                var flashcardDto = await flashcardsRepository.CreateFlashcard(newFlashcardDto, user);
                return Ok(flashcardDto);
            });
        });
    }

    [HttpGet("/api/decks/{deckId}/flashcards")]
    public async Task<ActionResult<IEnumerable<FlashcardDTO>>> GetFlashcardsByDeckId(string deckId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            var flashcardDtos = await flashcardsRepository.GetFlashcardsByDeckId(deckId, user);
            return Ok(flashcardDtos);
        });
    }

    [HttpPut("{flashcardId}")]
    public async Task<IActionResult> UpdateFlashcard(FlashcardDTO flashcardDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await flashcardsRepository.UpdateFlashcard(flashcardDto, user);
                return NoContent();
            });
        });
    }

    [HttpDelete("{flashcardId}")]
    public async Task<IActionResult> DeleteFlashcard(string flashcardId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await flashcardsRepository.DeleteFlashcard(flashcardId, user);
                return NoContent();
            });
        });
    }
}