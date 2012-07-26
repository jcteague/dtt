namespace TeamNotification_Library.Extensions
{
    public static class StringExtensions
    {
        public static string FormatUsing(this string str, params string[] values)
        {
            return string.Format(str, values);
        }
    }
}