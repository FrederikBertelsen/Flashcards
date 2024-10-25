using Backend.Exceptions;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DecksRepository(AppDbContext dbContext)
{
    public async Task<DeckDTO> CreateDeck(NewDeckDTO newDeckDto, User user)
    {
        await ValidateDeckName(newDeckDto.Name);

        var addedDeck = (await dbContext.Decks.AddAsync(newDeckDto.ToEntity(user))).Entity;
        await dbContext.SaveChangesAsync();

        return addedDeck.ToDto();
    }

    public async Task<IEnumerable<DeckDTO>> GetDecks(User? user)
    {
        var decksQuery = dbContext.Decks
            .Include(d => d.User)
            .OrderByDescending(d => d.CreatedAt)
            .AsQueryable();

        if (user == null)
            return await decksQuery
                .Where(d => d.IsPublic)
                .Include(d => d.Flashcards)
                .Select(d => d.ToDto())
                .ToListAsync();

        if (user.IsAdmin)
            return await decksQuery
                .Include(d => d.Flashcards)
                .Select(d => d.ToDto())
                .ToListAsync();

        var usersDeckIdSet = await GetUsersPrivateAccessDeckIdSet(user);
        return await decksQuery
            .Where(d => d.IsPublic || usersDeckIdSet.Contains(d.Id))
            .Include(d => d.Flashcards)
            .Select(d => d.ToDto())
            .ToListAsync();
    }

    public async Task<DeckDTO> GetDeckById(string deckId, User? user)
    {
        var deck = await dbContext.Decks
            .Include(d => d.User)
            .Include(d => d.Flashcards)
            .FirstOrDefaultAsync(s => s.Id == deckId);
        if (deck == null)
            throw new ModelNotFoundException($"Deck with id {deckId} not found.");

        if (!deck.IsPublic && !UserHasAccessToDeck(deck, user))
            throw new UnauthorizedAccessException("You do not have permission to view this deck.");

        return deck.ToDto();
    }

    public async Task<IEnumerable<DeckDTO>> GetDecksByUserId(string userId, User? user)
    {
        var foundUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (foundUser == null)
            throw new ModelNotFoundException($"User with id {userId} not found.");

        var decksQuery = dbContext.Decks
            .Where(d => d.UserId == userId)
            .Include(d => d.User)
            .AsQueryable();

        if (user == null)
            return await decksQuery
                .Where(d => d.IsPublic)
                .Include(d => d.Flashcards)
                .Select(d => d.ToDto())
                .ToListAsync();

        if (user.Id != userId && !user.IsAdmin)
        {
            var usersCollaborativeDeckIdsSet = await GetUsersCollaborativeDeckIdsSet(user);
            decksQuery = decksQuery
                .Where(d => d.IsPublic || usersCollaborativeDeckIdsSet.Contains(d.Id));
        }

        return await decksQuery
            .Include(d => d.Flashcards)
            .Select(d => d.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<DeckDTO>> GetDecksBySearchTerm(string searchTerm, User? user)
    {
        ValidateSearchTerm(searchTerm);

        var decksQuery = dbContext.Decks
            .Include(d => d.User)
            .GetMatches(searchTerm, d => d.Name)
            .AsQueryable();

        if (user == null)
            return await decksQuery
                .Where(d => d.IsPublic)
                .Include(d => d.Flashcards)
                .Select(d => d.ToDto())
                .ToListAsync();

        var usersDeckIdSet = await GetUsersPrivateAccessDeckIdSet(user);
        return await decksQuery
            .Where(d => d.IsPublic || usersDeckIdSet.Contains(d.Id))
            .Include(d => d.Flashcards)
            .Select(d => d.ToDto())
            .ToListAsync();
    }

    public async Task UpdateDeck(UpdateDeckDTO updateDeckDto, User user)
    {
        var deck = await dbContext.Decks.Include(d => d.User).FirstOrDefaultAsync(d => d.Id == updateDeckDto.Id);
        if (deck == null)
            throw new ModelNotFoundException($"Deck with id {updateDeckDto.Id} not found.");

        if (deck.User.Id != user.Id)
            throw new UnauthorizedAccessException("You can only update your own decks.");

        await ValidateDeckName(updateDeckDto.Name);

        deck.Name = updateDeckDto.Name;
        deck.IsPublic = updateDeckDto.IsPublic;
        
        dbContext.Decks.Update(deck);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteDeck(string deckId, User user)
    {
        var deck = await dbContext.Decks.Include(d => d.User).FirstOrDefaultAsync(d => d.Id == deckId);
        if (deck == null)
            throw new ModelNotFoundException($"Deck with id {deckId} not found");

        if (deck.User.Id != user.Id)
            throw new UnauthorizedAccessException("You can only delete your own decks.");

        dbContext.Decks.Remove(deck);
        await dbContext.SaveChangesAsync();
    }

    private async Task ValidateDeckName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Deck name cannot be empty.");
        if (name.Length is < 3 or > 64)
            throw new ArgumentException("Deck name must be between 3 and 64 characters long.");
        if (await dbContext.Users.AnyAsync(u => u.Name == name))
            throw new ModelAlreadyExistsException("Deck with this name already exists.");
    }

    private async Task<HashSet<string>> GetUsersPrivateAccessDeckIdSet(User user)
    {
        var usersCollaborativeDeckIds = await dbContext.DeckCollaborators
            .Where(dc => dc.UserId == user.Id)
            .Select(dc => dc.DeckId)
            .ToListAsync();
        var usersDeckId = await dbContext.Decks
            .Where(d => d.UserId == user.Id)
            .Select(d => d.Id)
            .ToListAsync();

        return usersCollaborativeDeckIds
            .Concat(usersDeckId)
            .ToHashSet();
    }

    private async Task<HashSet<string>> GetUsersCollaborativeDeckIdsSet(User user)
    {
        return (await dbContext.DeckCollaborators
                .Where(dc => dc.UserId == user.Id)
                .Select(dc => dc.DeckId)
                .ToListAsync())
            .ToHashSet();
    }

    private void ValidateSearchTerm(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            throw new ArgumentException("Search term cannot be empty.");

        if (searchTerm.Length is < 3 or > 64)
            throw new ArgumentException("Search term must be between 3 and 64 characters long.");
    }

    private bool UserHasAccessToDeck(Deck deck, User? user)
    {
        if (user == null)
            return false;
        
        if (deck.IsPublic || deck.UserId == user.Id || user.IsAdmin)
            return true;

        var isCollaborator = dbContext.DeckCollaborators
            .Any(dc => dc.DeckId == deck.Id && dc.UserId == user.Id);
        return isCollaborator;
    }
}