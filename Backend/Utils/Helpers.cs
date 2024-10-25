using Backend.Models;

namespace Backend.Utils;

public static class Helpers
{
    public static List<Review> GetDueReviews(this ICollection<Review> reviews) =>
        reviews.Where(review => review.Card.Due < DateTime.Now).ToList();

    // public static List<Review> CreateReviews(this ICollection<Flashcard> flashcards, LearningSession learningSession)
    // {
    //     return flashcards.Select(f => new Review
    //     {
    //         LearningSessionId = learningSession.Id,
    //         LearningSession = learningSession,
    //         FlashcardId = f.Id,
    //         Flashcard = f,
    //         DueDate = DateTime.Now,
    //         Stability = 2.5f,
    //         Difficulty = 2.5f,
    //         ElapsedDays = 0,
    //         ScheduledDays = 0,
    //         Reps = 0,
    //         Lapses = 0,
    //         State = State.New,
    //         ClozeIndex = 0 //todo: implement cloze index
    //     }).ToList();
    // }
}