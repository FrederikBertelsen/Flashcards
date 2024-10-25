using System.Text;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Repositories;

namespace Backend.Utils;

public static class DBSeeder
{
    public static async Task Seed(this AppDbContext dbContext)
    {
        if (dbContext.Users.Any() || dbContext.Decks.Any() || dbContext.Flashcards.Any())
            return;

        ClearDatabase(dbContext);

        var importRepository = new ImportRepository(dbContext);

        var quizletDeckIds = new List<string>
        {
            "76397882",
            "927714346",
            "924767755",
            "922614813",
            "297245653"
        };

        var testUsers = GenerateUsers(quizletDeckIds.Count).ToList();

        for (int i = 0; i < quizletDeckIds.Count; i++)
        {
            try
            {
                await importRepository.ImportQuizletDeck(quizletDeckIds[i], testUsers[i]);
                
            } catch (Exception e)
            {
                Console.WriteLine($"Error with quizlet deck '{quizletDeckIds[i]}': {e.Message}");
            }
            // await Task.Delay(10000);
        }
    }

    public static async void SeedRandom(this AppDbContext dbContext)
    {
        if (dbContext.Users.Any() || dbContext.Decks.Any() || dbContext.Flashcards.Any())
            return;


        ClearDatabase(dbContext);

        var testUsers = GenerateUsers(10).ToList();
        var testDecks = GenerateDecks(testUsers, 30).ToList();
        var flashcards = GenerateFlashcards(testDecks, 10, 500).ToList();

        dbContext.Users.AddRange(testUsers);
        dbContext.Decks.AddRange(testDecks);
        dbContext.Flashcards.AddRange(flashcards);

        await dbContext.SaveChangesAsync();
    }

    private static void ClearDatabase(AppDbContext dbContext)
    {
        dbContext.Flashcards.RemoveRange(dbContext.Flashcards);
        dbContext.Decks.RemoveRange(dbContext.Decks);
        dbContext.Users.RemoveRange(dbContext.Users);
        dbContext.DeckCollaborators.RemoveRange(dbContext.DeckCollaborators);
        dbContext.LearningSessions.RemoveRange(dbContext.LearningSessions);
        dbContext.Reviews.RemoveRange(dbContext.Reviews);
        dbContext.ReviewLogs.RemoveRange(dbContext.ReviewLogs);
        // dbContext.Sessions.RemoveRange(dbContext.Sessions);
    }

    private static IEnumerable<User> GenerateUsers(int count)
    {
        return Enumerable.Range(1, count).Select(i => new User
        {
            Name = $"Test User {i}",
            GoogleId = $"googleId {i}",
            PictureUrl = ""
        });
    }

    private static IEnumerable<Deck> GenerateDecks(IEnumerable<User> users, int count)
    {
        var userList = users.ToList();
        return Enumerable
            .Range(1, count)
            .Select(i => new Deck
            {
                Name = $"Test Deck {i}",
                IsPublic = i % 4 == 0,
                UserId = userList[i % userList.Count].Id,
                User = userList[i % userList.Count]
            });
    }

    private static List<Flashcard> GenerateFlashcards(IEnumerable<Deck> decks, int min, int max)
    {
        var random = new Random();
        var flashcards = new List<Flashcard>();

        foreach (var deck in decks)
        {
            var testFlashcards = Enumerable
                .Range(1, random.Next(min, max))
                .Select(_ => new Flashcard
                {
                    Deck = deck,
                    DeckId = deck.Id,
                    Front = GenerateText(),
                    Back = GenerateText(),
                    FlashType = random.Next(0, 2) == 0 ? FlashType.Normal : FlashType.Cloze
                });

            flashcards.AddRange(testFlashcards);
        }

        return flashcards;
    }

    private static string GenerateText()
    {
        var words = new[]
        {
            "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit", "sed", "do", "eiusmod",
            "tempor", "incididunt", "ut", "labore", "et", "dolore", "magna", "aliqua", "ut", "enim", "ad", "minim",
            "veniam", "quis", "nostrud", "exercitation", "ullamco", "laboris", "nisi", "ut", "aliquip", "ex", "ea",
            "commodo", "consequat", "duis", "aute", "irure", "dolor", "in", "reprehenderit", "in", "voluptate",
            "velit", "esse", "cillum", "dolore", "eu", "fugiat", "nulla", "pariatur", "excepteur", "sint", "occaecat",
            "cupidatat", "non", "proident", "sunt", "in", "culpa", "qui", "officia", "deserunt", "mollit", "anim",
            "id", "est", "laborum", ",", ",", ",", ".", "?", ":"
        };

        var rand = new Random();
        var textLength = rand.Next(5, 40);
        var newLineCount = textLength > 20 ? -2 : 0;

        var result = new StringBuilder();

        for (var i = 0; i < textLength; i++)
        {
            if (newLineCount < 5 && rand.Next(0, 20) == 0)
            {
                result.Append('\n');
                newLineCount++;
            }
            else
            {
                result.Append(words[rand.Next(words.Length)]).Append(' ');
            }
        }

        return result.ToString().Trim();
    }
}