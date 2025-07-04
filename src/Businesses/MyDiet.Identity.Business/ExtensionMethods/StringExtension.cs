namespace System
{
    public static class StringExtension
    {
        public static string LowercaseFirst(this string s)
        {
            return char.ToLowerInvariant(s[0]) + s[1..];
        }
    }
}
