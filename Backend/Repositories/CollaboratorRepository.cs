using Backend.Exceptions;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class CollaboratorRepository(AppDbContext dbContext)
{
    public async Task<IEnumerable<CollaboratorDTO>> GetDeckCollaborators(string deckId, User? user)
    {
        var deck = await dbContext.Decks
            .FirstOrDefaultAsync(d => d.Id == deckId);
        if (deck == null)
            throw new ModelNotFoundException($"Deck with id {deckId} not found.");

        var collaborators = dbContext.DeckCollaborators
            .Include(dc => dc.User)
            .Where(dc => dc.DeckId == deckId)
            .Select(dc => dc.ToDto())
            .ToList();


        if (!deck.IsPublic &&
            (user is null ||
             deck.UserId != user.Id && collaborators.All(c => c.Id != user.Id) && !user.IsAdmin))
            throw new UnauthorizedAccessException("You do not have permission to view this deck's collaborators.");

        return collaborators;
    }

    public async Task UpdateDeckCollaborators(string deckId, string[] collaboratorIds, User user)
    {
        var collaboratorIdSet = collaboratorIds.Distinct().ToHashSet();

        if (collaboratorIdSet.Contains(user.Id))
            throw new ArgumentException("You cannot add yourself as a collaborator.");

        var deck = await dbContext.Decks
            .FirstOrDefaultAsync(d => d.Id == deckId);
        if (deck == null)
            throw new ModelNotFoundException($"Deck with id {deckId} not found.");

        var existingCollaborators = await dbContext.DeckCollaborators
            .Where(dc => dc.DeckId == deckId)
            .ToListAsync();
        var newCollaborators = await dbContext.Users
            .Where(u => collaboratorIdSet.Contains(u.Id))
            .Select(u => new DeckCollaborator
            {
                Deck = deck,
                DeckId = deck.Id,
                User = u,
                UserId = u.Id
            }).ToListAsync();

        var foundCollaboratorIds = newCollaborators.Select(nc => nc.UserId).ToHashSet();
        var missingCollaboratorIds = collaboratorIdSet.Except(foundCollaboratorIds).ToList();
        if (missingCollaboratorIds.Count != 0)
            throw new ArgumentException(
                $"The following User IDs were not found: {string.Join(", ", missingCollaboratorIds)}");

        var collaboratorsToRemove = existingCollaborators
            .Where(ec => newCollaborators.All(nc => nc.UserId != ec.UserId))
            .ToList();
        var collaboratorsToAdd = newCollaborators
            .Where(nc => existingCollaborators.All(ec => ec.UserId != nc.UserId))
            .ToList();

        dbContext.DeckCollaborators.RemoveRange(collaboratorsToRemove);
        await dbContext.DeckCollaborators.AddRangeAsync(collaboratorsToAdd);
        await dbContext.SaveChangesAsync();
    }
}