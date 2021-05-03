using System.Text.RegularExpressions;

namespace CookBook.Infrastructure
{
    public static class StringExtensions
    {
        public static string Canonicalize(this string s) => Regex.Replace(s.Trim(), "[^a-zA-Z0-9]", "-").ToLower();
    }
}