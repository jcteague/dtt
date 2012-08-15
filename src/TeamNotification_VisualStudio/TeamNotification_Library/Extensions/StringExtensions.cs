namespace TeamNotification_Library.Extensions
{
    public static class StringExtensions
    {
        public static string FormatUsing(this string str, params string[] values)
        {
            return string.Format(str, values);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}