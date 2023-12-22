namespace server;

public static class ValueExtensions
{
    public static int ConvertToInt(this string value)
    {
        return int.TryParse(value, out var result) ? result : 0;
    }
}