using System.Text.Json;
using Backend.Models;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Utils;

namespace Backend.Mappers;

public static class Mapper
{
    #region Session mappings
    public static Session ToEntity(this NewSessionDTO newSessionDto, User user) =>
        new()
        {
            UserId = user.Id,
            Id = newSessionDto.Id,
            ExpiresAt = newSessionDto.ExpiresAt
        };

    public static SessionDTO ToDto(this Session session) =>
        new()
        {
            Id = session.Id,
            UserId = session.UserId,
            ExpiresAt = session.ExpiresAt,
            Fresh = session.Fresh
        };
    #endregion

    #region User mappings
    public static User ToEntity(this UserDTO userDto) =>
        new()
        {
            Id = userDto.Id,
            Name = userDto.Name,
            GoogleId = userDto.GoogleId,
            PictureUrl = userDto.PictureUrl
        };

    public static User ToEntity(this NewUserDTO newUserDto) =>
        new()
        {
            Name = newUserDto.Name,
            GoogleId = newUserDto.GoogleId,
            PictureUrl = newUserDto.PictureUrl
        };

    public static UserDTO ToDto(this User user) =>
        new()
        {
            Id = user.Id,
            Name = user.Name,
            IsAdmin = user.IsAdmin,
            GoogleId = user.GoogleId,
            PictureUrl = user.PictureUrl
        };
    #endregion

    #region Deck mappings
    public static Deck ToEntity(this NewDeckDTO newDeckDto, User creator) =>
        new()
        {
            Name = newDeckDto.Name,
            UserId = creator.Id,
            User = creator
        };

    public static DeckDTO ToDto(this Deck deck)
    {
        if (deck.User is null)
            throw new InvalidOperationException("Deck creator must be loaded to map to DTO");

        return new DeckDTO
        {
            Id = deck.Id,
            Name = deck.Name,
            IsPublic = deck.IsPublic,
            FlashcardCount = deck.Flashcards.Count,
            Creator = deck.User.ToDto(),
        };
    }
    #endregion

    #region Flashcard mappings
    public static Flashcard ToEntity(this NewFlashcardDTO newFlashcardDto, Deck deck) =>
        new()
        {
            Deck = deck,
            DeckId = deck.Id,
            FlashType = newFlashcardDto.FlashType,
            Front = newFlashcardDto.Front,
            Back = newFlashcardDto.Back
        };

    public static FlashcardDTO ToDto(this Flashcard flashcard) =>
        new()
        {
            Id = flashcard.Id,
            FlashType = flashcard.FlashType,
            Front = flashcard.Front,
            Back = flashcard.Back ?? "",
        };
    #endregion
    
    #region LearningSession mappings
    public static LearningSession ToEntity(this NewLearningSessionDTO newLearningSessionDto, User user, Deck deck) =>
        new()
        {
            Name = newLearningSessionDto.Name,
            UserId = user.Id,
            User = user,
            DeckId = deck.Id,
            Deck = deck,
        };
    
    public static LearningSessionDTO ToDto(this LearningSession learningSession) =>
        new()
        {
            Id = learningSession.Id,
            Name = learningSession.Name,
            DeckId = learningSession.DeckId,
            ReviewCount = learningSession.Reviews.Count,
            DueReviewCount = learningSession.Reviews.GetDueReviews().Count
        };
    #endregion
    
    #region Review mappings
    public static Review ToEntity(this NewReviewDto newReviewDto, LearningSession learningSession) =>
        new()
        {
            LearningSession = learningSession,
            LearningSessionId = learningSession.Id,
            FlashcardId = newReviewDto.FlashcardId,
            Flashcard = learningSession.Deck.Flashcards.First(f => f.Id == newReviewDto.FlashcardId),
            ClozeIndex = newReviewDto.ClozeIndex,
            Card = newReviewDto.Card
        };
    
    public static ICollection<Review> ToEntities(this ICollection<NewReviewDto> newReviewDtos, LearningSession learningSession) =>
        newReviewDtos
            .Select(dto => dto.ToEntity(learningSession))
            .ToList();
    public static ReviewDTO ToDto(this Review review) =>
        new()
        {
            Id = review.Id,
            LearningSessionId = review.LearningSessionId,
            ClozeIndex = review.ClozeIndex,
            Card = review.Card
        };
    
    public static ReviewLog ToEntity(this NewReviewLogDTO newReviewLogDto, Review review, LearningSession learningSession) =>
        new()
        {
            LearningSessionId = learningSession.Id,
            LearningSession = learningSession,
            ReviewId = review.Id,
            Review = review,
            Log = newReviewLogDto.Log
        };
    #endregion

    #region Collaborator mappings
    public static CollaboratorDTO ToDto(this DeckCollaborator deckCollaborator) =>
        new()
        {
            Id = deckCollaborator.UserId,
            Name = deckCollaborator.User.Name,
            PictureUrl = deckCollaborator.User.PictureUrl
        };
    #endregion
}
