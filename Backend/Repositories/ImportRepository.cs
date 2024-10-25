using System.Text.Json;
using Backend.Mappers;
using Backend.Models;
using Backend.Models.DbContext;
using Backend.Models.DTOs;

namespace Backend.Repositories;

public class ImportRepository(AppDbContext dbContext)
{
    private const string DefaultUserAgent =
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36";

    public async Task<DeckDTO> ImportQuizletDeck(string quizletDeckId, User user)
    {
        // Create deck
        string deckName = await GetQuizletDeckNameAsync(quizletDeckId);
        Deck newDeck = new Deck
        {
            Name = deckName,
            UserId = user.Id,
            User = user
        };
        Deck addedDeck = (await dbContext.Decks.AddAsync(newDeck)).Entity;
        await dbContext.SaveChangesAsync();

        // Create flashcards
        List<Flashcard> flashcards = new();
        var flashcardPairs = await GetQuizletFlashcardsAsync(quizletDeckId);
        foreach (var (front, back) in flashcardPairs)
        {
            Flashcard flashcard = new Flashcard
            {
                FlashType = FlashType.Normal,
                Front = front,
                Back = back,
                DeckId = addedDeck.Id,
                Deck = addedDeck
            };
            flashcards.Add(flashcard);
        }

        await dbContext.Flashcards.AddRangeAsync(flashcards);
        await dbContext.SaveChangesAsync();

        return addedDeck.ToDto();
    }

    private async Task<string> GetQuizletDeckNameAsync(string quizletDeckId)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);

        string deckUrl = $"https://quizlet.com/webapi/3.9/sets/{quizletDeckId}";
        string response = await httpClient.GetStringAsync(deckUrl);

        using JsonDocument document = JsonDocument.Parse(response);
        JsonElement root = document.RootElement;
        if (root.TryGetProperty("responses", out JsonElement responsesElement) &&
            responsesElement[0].TryGetProperty("models", out JsonElement modelsElement) &&
            modelsElement.TryGetProperty("set", out JsonElement setElement) &&
            setElement[0].TryGetProperty("title", out JsonElement titleElement))
        {
            return titleElement.GetString();
        }

        return $"Quizlet Deck {quizletDeckId}";
    }

    private async Task<List<(string Front, string Back)>> GetQuizletFlashcardsAsync(string quizletDeckId)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgent);

        var flashcards = new List<(string Front, string Back)>();
        int page = 1;
        bool hasMoreFlashcards = true;

        while (hasMoreFlashcards)
        {
            string flashcardsUrl =
                $"https://quizlet.com/webapi/3.9/studiable-item-documents" +
                $"?filters[studiableContainerId]={quizletDeckId}" +
                $"&filters[studiableContainerType]=1" +
                $"&perPage=1000&page={page}";
            
            // Console.WriteLine(flashcardsUrl);

            string response = await httpClient.GetStringAsync(flashcardsUrl);
            
            using JsonDocument document = JsonDocument.Parse(response);
            JsonElement root = document.RootElement;
            if (root.TryGetProperty("responses", out JsonElement responsesElement) &&
                responsesElement[0].TryGetProperty("models", out JsonElement modelsElement) &&
                modelsElement.TryGetProperty("studiableItem", out JsonElement flashcardsElement))
            {
                int flashcardsOnPage = 0;
                foreach (var flashcard in flashcardsElement.EnumerateArray())
                {
                    string frontText = string.Empty;
                    string backText = string.Empty;

                    if (flashcard.TryGetProperty("cardSides", out JsonElement cardSidesElement))
                    {
                        foreach (var side in cardSidesElement.EnumerateArray())
                        {
                            if (side.TryGetProperty("label", out JsonElement labelElement) &&
                                side.TryGetProperty("media", out JsonElement mediaElement))
                            {
                                string label = labelElement.GetString();
                                string text = mediaElement[0].GetProperty("plainText").GetString();

                                if (label is null || text is null)
                                    continue;

                                if (label == "word")
                                    frontText = text;
                                else if (label == "definition")
                                    backText = text;
                            }
                        }
                    }

                    flashcards.Add((frontText, backText));
                    flashcardsOnPage++;
                }

                hasMoreFlashcards = flashcardsOnPage >= 1000;
                page++;
            }
            else
            {
                hasMoreFlashcards = false;
            }
        }

        return flashcards;
    }
}