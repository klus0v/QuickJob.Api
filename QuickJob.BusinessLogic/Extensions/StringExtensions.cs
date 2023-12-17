namespace QuickJob.BusinessLogic.Extensions;

public static class StringExtensions
{
    private const char DefaultSeparator = ',';
    
    public static bool TryGetList(this string? str, out List<string> list, char? separator = null)
    {
        if (!string.IsNullOrEmpty(str)) 
        {
            list = str.Split(separator ?? DefaultSeparator).ToList();
            return true;
        }
        list = new List<string>();
        return false;
    }
}