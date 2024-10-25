using System.ComponentModel.DataAnnotations;
using Backend.Exceptions;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class LearningSessionRepository(AppDbContext dbContext)
{

    public async Task<IEnumerable<LearningSessionDTO>> GetLearningSessionsByUserId(string userId, User user)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty", nameof(userId));

        var foundUser = await dbContext.Users.FindAsync(userId);
        if (foundUser == null)
            throw new ModelNotFoundException($"User with ID '{userId}' not found");

        if (foundUser.Id != user.Id && !user.IsAdmin)
            throw new UnauthorizedAccessException();

        var learningSessions = await dbContext.LearningSessions
            .Where(ls => ls.UserId == userId)
            .ToListAsync();

        return learningSessions.Select(ls => ls.ToDto());
    }

    public async Task<LearningSessionDTO> CreateLearningSession(NewLearningSessionDTO newLearningSessionDto, User user)
    {
        if (newLearningSessionDto.Reviews.Count == 0)
            throw new ArgumentException("Learning session must have at least one review");

        if (newLearningSessionDto.Name.Length > Constants.MaxNameLength)
            throw new ArgumentException($"Learning Session name exceeds maximum length of {Constants.MaxNameLength}");

        if (user.Id != newLearningSessionDto.UserId)
            throw new UnauthorizedAccessException();

        var deck = await dbContext.Decks.FindAsync(newLearningSessionDto.DeckId);
        if (deck == null)
            throw new ModelNotFoundException($"Deck with ID '{newLearningSessionDto.DeckId}' not found");
        
        var learningSession = newLearningSessionDto.ToEntity(user, deck);
        var reviews = newLearningSessionDto.Reviews.ToEntities(learningSession);

        await dbContext.LearningSessions.AddAsync(learningSession);
        await dbContext.Reviews.AddRangeAsync(reviews);
        await dbContext.SaveChangesAsync();

        return learningSession.ToDto();
    }

    public async Task<IEnumerable<ReviewDTO>> GetDueReviewsByLearningSessionId(string learningSessionId, User user)
    {
        if (string.IsNullOrWhiteSpace(learningSessionId))
            throw new ArgumentException("Learning Session ID cannot be null or empty", nameof(learningSessionId));

        var learningSession = await dbContext.LearningSessions
            .Include(ls => ls.Reviews)
            .SingleOrDefaultAsync(ls => ls.Id == learningSessionId);
        if (learningSession == null)
            throw new ModelNotFoundException($"Learning session with ID '{learningSessionId}' not found");

        if (learningSession.UserId != user.Id && !user.IsAdmin)
            throw new UnauthorizedAccessException();

        var dueReviews = learningSession.Reviews.GetDueReviews();

        return dueReviews.Select(r => r.ToDto());
    }

    public async Task CreateReview(NewReviewDto newReviewDto, User user)
    {
        var learningSession = await dbContext.LearningSessions
            .SingleOrDefaultAsync(ls => ls.Id == newReviewDto.LearningSessionId);
        if (learningSession == null)
            throw new ModelNotFoundException($"Learning session with ID '{newReviewDto.LearningSessionId}' not found");
        if (learningSession.UserId != user.Id)
            throw new UnauthorizedAccessException();

        var flashcard = await dbContext.Flashcards.FindAsync(newReviewDto.FlashcardId);
        if (flashcard == null)
            throw new ModelNotFoundException($"Flashcard with ID '{newReviewDto.FlashcardId}' not found");
        if (flashcard.DeckId != learningSession.DeckId)
            throw new ArgumentException("Flashcard does not belong to learning session's deck");

        var review = newReviewDto.ToEntity(learningSession);

        await dbContext.Reviews.AddAsync(review);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateReview(string reviewId, Card card, User user)
    {
        var review = await dbContext.Reviews.FindAsync(reviewId);
        if (review == null)
            throw new ModelNotFoundException($"Review with ID '{reviewId}' not found");

        var learningSession = await dbContext.LearningSessions
            .SingleOrDefaultAsync(ls => ls.Id == review.LearningSessionId);
        if (learningSession == null)
            throw new ModelNotFoundException($"Learning session with ID '{review.LearningSessionId}' not found");

        if (learningSession.UserId != user.Id)
            throw new UnauthorizedAccessException();

        review.Card = card;
        
        await dbContext.SaveChangesAsync();
    }

    public async Task CreateReviewLog(NewReviewLogDTO newReviewLogDto, User user)
    {
        var review = await dbContext.Reviews.FindAsync(newReviewLogDto.ReviewId);
        if (review == null)
            throw new ModelNotFoundException($"Review with ID '{newReviewLogDto.ReviewId}' not found");

        var learningSession = await dbContext.LearningSessions
            .SingleOrDefaultAsync(ls => ls.Id == review.LearningSessionId);
        if (learningSession == null)
            throw new ModelNotFoundException($"Learning session with ID '{review.LearningSessionId}' not found");

        if (learningSession.UserId != user.Id)
            throw new UnauthorizedAccessException();

        var reviewLog = newReviewLogDto.ToEntity(review, learningSession);

        await dbContext.ReviewLogs.AddAsync(reviewLog);
        await dbContext.SaveChangesAsync();
    }
}