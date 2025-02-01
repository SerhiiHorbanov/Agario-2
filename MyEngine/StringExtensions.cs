namespace MyEngine;

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
}