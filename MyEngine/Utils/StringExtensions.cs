namespace MyEngine.Utils;

public static class StringExtensions
{
    public static string TrimAfter(this string str, char trim)
        => str.TrimAfter(trim.ToString());
    
    public static string TrimAfter(this string str, string trim)
    {
        int index = str.IndexOf(trim);
        
        if (index == -1)
            return str;

        return str.Remove(index, str.Length - 1);
    }

    public static string TrimFirstLast(this string str, char ch)
        => str.TrimFirstLast(ch, ch);
    
    public static string TrimFirstLast(this string str, char first, char last)
    {
        int substrLen = str.Length;
        int substrBegin = 0;
        
        if (str[0] == first)
        {
            substrLen--;
            substrBegin = 1;
        }
        if (str[^1] == last)
            substrLen--;
        
        return str.Substring(substrBegin, substrLen);
    }

    public static string TrimBrackets(this string str)
        => str.Trim('(', ')', '{', '}', '[', ']', '<', '>');
}