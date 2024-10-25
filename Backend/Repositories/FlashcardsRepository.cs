using Backend.Exceptions;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class FlashcardsRepository(AppDbContext dbContext)
{
    public async Task<FlashcardDTO> CreateFlashcard(NewFlashcardDTO newFlashcardDto, User user)
    {
        var deck = await dbContext.Decks
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == newFlashcardDto.DeckId);

        if (deck == null)
            throw new ModelNotFoundException($"Deck with id {newFlashcardDto.DeckId} not found");

        var isCollaborator = await dbContext.DeckCollaborators
            .AnyAsync(dc => dc.DeckId == deck.Id && dc.UserId == user.Id);
        
        if (deck.User.Id != user.Id && !isCollaborator)
            throw new UnauthorizedAccessException("You do not have permission to add flashcards to this deck.");

        ValidateSides(newFlashcardDto.FlashType, newFlashcardDto.Front, newFlashcardDto.Back);

        var addedFlashcard = (await dbContext.Flashcards.AddAsync(newFlashcardDto.ToEntity(deck))).Entity;

        await dbContext.SaveChangesAsync();

        return addedFlashcard.ToDto();
    }

    public async Task<IEnumerable<FlashcardDTO>> GetFlashcardsByDeckId(string deckId, User? user)
    {
        var deck = await dbContext.Decks
            .Include(d => d.Flashcards)
            .FirstOrDefaultAsync(d => d.Id == deckId);

        if (deck == null)
            throw new ModelNotFoundException($"Deck with id {deckId} not found");

        if (!deck.IsPublic)
        {
            if (user == null)
                throw new UnauthorizedAccessException("You need to be logged in to access this deck's flashcards.");
            
            var isCollaborator = await dbContext.DeckCollaborators
                .AnyAsync(dc => dc.DeckId == deck.Id && dc.UserId == user.Id);
            
            if (deck.UserId != user.Id && !isCollaborator && !user.IsAdmin)
                throw new UnauthorizedAccessException("You do not have permission to view this deck's flashcards.");
        }

        return deck.Flashcards.OrderByDescending(f => f.CreatedAt).Select(d => d.ToDto()).ToList();
    }

    public async Task UpdateFlashcard(FlashcardDTO flashcardDto, User user)
    {
        var flashcard = await dbContext.Flashcards
            .Include(f => f.Deck)
            .Include(f => f.Deck.User)
            .SingleOrDefaultAsync(f => f.Id == flashcardDto.Id);

        if (flashcard == null)
            throw new ModelNotFoundException($"Flashcard with id {flashcardDto.Id} not found");
        
        var isCollaborator = await dbContext.DeckCollaborators
            .AnyAsync(dc => dc.DeckId == flashcard.DeckId && dc.UserId == user.Id);

        if (flashcard.Deck.User.Id != user.Id && !isCollaborator)
            throw new UnauthorizedAccessException("You do not have permission to update this flashcard.");

        ValidateSides(flashcardDto.FlashType, flashcardDto.Front, flashcardDto.Back);

        flashcard.FlashType = flashcardDto.FlashType;
        flashcard.Front = flashcardDto.Front;
        flashcard.Back = flashcardDto.Back;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteFlashcard(string flashcardId, User user)
    {
        var flashcard = await dbContext.Flashcards
            .Include(f => f.Deck)
            .Include(f => f.Deck.User)
            .FirstOrDefaultAsync(f => f.Id == flashcardId);
        if (flashcard == null)
            throw new ModelNotFoundException($"Flashcard with id {flashcardId} not found");

        
        var isCollaborator = await dbContext.DeckCollaborators
            .AnyAsync(dc => dc.DeckId == flashcard.DeckId && dc.UserId == user.Id);
        
        if (flashcard.Deck.User.Id != user.Id && !isCollaborator)
            throw new UnauthorizedAccessException("You do not have permission to delete this flashcard.");

        dbContext.Flashcards.Remove(flashcard);
        await dbContext.SaveChangesAsync();
    }

    private void ValidateSides(FlashType flashType, string front, string back)
    {
        if (string.IsNullOrWhiteSpace(front))
            if (flashType == FlashType.Normal)
                throw new ArgumentException("Front of flashcard cannot be empty.");
            else
                throw new ArgumentException("Cloze of flashcard cannot be empty.");
        if (flashType == FlashType.Normal && string.IsNullOrWhiteSpace(back))
            throw new ArgumentException("Back of flashcard cannot be empty.");

        if (front.Length > 512)
            if (flashType == FlashType.Normal)
                throw new ArgumentException("Front of flashcard cannot be over 512 characters long.");
            else
                throw new ArgumentException("Cloze of flashcard cannot be over 512 characters long.");
        if (flashType == FlashType.Normal && string.IsNullOrWhiteSpace(back))
            throw new ArgumentException("Back of flashcard cannot be over 512 characters long.");
    }
}