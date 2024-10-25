using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Backend.Utils;

public static partial class UniqueIdGenerator
{
    public static string Generate()
    {
        byte[] hashBytes = SHA256.HashData(Guid.NewGuid().ToByteArray());
        string base64 = Convert.ToBase64String(hashBytes);

        // Remove any character that isn't a number or letter
        string cleanedId = LettersNumbersRegex().Replace(base64, "");

        // Take the first n characters for a very short ID
        return cleanedId[..Constants.MaxIdLength];
    }

    [GeneratedRegex("[^a-zA-Z0-9]")]
    private static partial Regex LettersNumbersRegex();
}