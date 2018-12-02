namespace Base.Utils
{
    public static class StringExtensions
    {
        public static string OrIfNullOrEmtry(this string str, string other)
        {
            return string.IsNullOrWhiteSpace(str) ? other : str;
        }
    }
}
