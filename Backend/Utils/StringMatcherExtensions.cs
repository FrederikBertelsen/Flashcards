using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Backend.Utils
{
    public static class StringMatcherExtensions
    {
        public static IQueryable<T> GetMatches<T>(
            this IQueryable<T> query,
            string searchTerm,
            Func<T, string> stringSelector
        )
        {
            // Fetch data from database first
            var objectList = query.ToList();

            // Utilize StringMatcher's GetMatches method
            return StringMatcher<T>.GetMatches(searchTerm, objectList, stringSelector).AsQueryable();
        }
    }
}