using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Backend.Utils
{
    public class StringMatcher<T>
    {
        private static double CalcMinimumScore(string searchTerm) => 0.5 * searchTerm.Length;

        public static List<T> GetMatches(
            string searchTerm,
            List<T> objectList,
            Func<T, string> stringSelector)
        {
            searchTerm = searchTerm.ToLower();
            char[] searchTermArray = searchTerm.ToCharArray();

            double minimumScore = CalcMinimumScore(searchTerm);

            var matchedList = new List<(T, int)>();

            foreach (var obj in objectList)
            {
                string? str = stringSelector(obj)?.ToLower();
                if (str == null) 
                    continue;

                int score = str.Contains(searchTerm) 
                    ? int.MaxValue 
                    : CalcStringMatch(searchTermArray, str.ToCharArray());

                if (score >= minimumScore) 
                    matchedList.Add((obj, score));
            }

            matchedList.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            return matchedList
                .Select(m => m.Item1)
                .ToList();
        }

        private static int CalcStringMatch(char[] stringA, char[] stringB)
        {
            char[] longString;
            char[] shortString;
            if (stringA.Length > stringB.Length)
            {
                longString = stringA;
                shortString = stringB;
            }
            else
            {
                longString = stringB;
                shortString = stringA;
            }

            int lengthDiff = longString.Length - shortString.Length;
            if (lengthDiff == 0)
                return CalcStringLikeness(longString, shortString, true);

            int highestScore = 0;
            for (int i = 0; i < lengthDiff; i++)
            {
                int newScore = CalcStringLikeness(longString, shortString, i, 0, false);
                int newScoreCutBoth = CalcStringLikeness(longString, shortString, i, 0, true);

                if (newScore > highestScore)
                    highestScore = newScore;
                if (newScoreCutBoth > highestScore)
                    highestScore = newScoreCutBoth;
            }

            return highestScore;
        }

        private static int CalcStringLikeness(char[] longCharArray, char[] shortCharArray, bool cutBoth) =>
            CalcStringLikeness(longCharArray, shortCharArray, 0, 0, cutBoth);

        private static int CalcStringLikeness(char[] longCharArray, char[] shortCharArray, int lStart, int sStart, bool cutBoth)
        {
            int score = 0;
            int lIndex = lStart, sIndex = sStart;
            int lLength = longCharArray.Length;
            int sLength = shortCharArray.Length;

            while (lIndex < lLength && sIndex < sLength)
            {
                int lRemaining = lLength - lIndex;
                int sRemaining = sLength - sIndex;

                // 12345
                // x
                // x
                if (longCharArray[lIndex] == shortCharArray[sIndex])
                {
                    if (lRemaining > 1 && sRemaining > 1)
                    {
                        // 12345
                        // xx
                        // xy
                        if (longCharArray[lIndex + 1] == shortCharArray[sIndex] && longCharArray[lIndex] != shortCharArray[sIndex + 1])
                        {
                            score += 2;
                            if (lRemaining == 2)
                                return score;
                            lIndex += 2;
                            sIndex++;
                            continue;
                        }
                        
                        // 12345
                        // xy
                        // xx
                        if (longCharArray[lIndex] == shortCharArray[sIndex + 1] && longCharArray[lIndex + 1] != shortCharArray[sIndex])
                        {
                            score += 2;
                            if (sRemaining == 2)
                                return score;
                            lIndex++;
                            sIndex += 2;
                            continue;
                        }
                    }

                    // 12345
                    // x
                    // x
                    score += 3;
                    if (lRemaining <= 1 || sRemaining <= 1)
                        return score;
                    lIndex++;
                    sIndex++;
                    continue;
                }

                if (sRemaining > 1)
                {
                    // 12345
                    // xy
                    // yx
                    if (lRemaining > 1 && longCharArray[lIndex] == shortCharArray[sIndex + 1] && longCharArray[lIndex + 1] == shortCharArray[sIndex])
                    {
                        score += 2;
                        if (lRemaining == 2 || sRemaining == 2)
                            return score;
                        lIndex += 2;
                        sIndex += 2;
                        continue;
                    }

                    // 12345
                    // x
                    //  x
                    if (longCharArray[lIndex] == shortCharArray[sIndex + 1])
                    {
                        score += 2;
                        if (lRemaining <= 1 || sRemaining == 2)
                            return score;
                        lIndex++;
                        sIndex += 2;
                        continue;
                    }
                }

                // 12345
                //  x
                // x
                if (lRemaining > 1 && longCharArray[lIndex + 1] == shortCharArray[sIndex])
                {
                    score += 2;
                    if (lRemaining == 2 || sRemaining <= 1)
                        return score;
                    lIndex += 2;
                    sIndex++;
                    continue;
                }

                if (lRemaining <= 1 || sRemaining <= 1)
                    return score;

                // no match
                score -= 3;
                lIndex++;
                if (cutBoth)
                    sIndex++;
            }

            return score;
        }
    }
}