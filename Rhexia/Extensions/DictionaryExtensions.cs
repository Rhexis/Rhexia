namespace Rhexia.Extensions;

public static class DictionaryExtensions
{
    public static bool TrySet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        switch (dict.ContainsKey(key))
        {
            case true:
                dict[key] = value;
                return true;
            default:
                return false;
        }
    }
}