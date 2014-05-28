namespace Grove.Infrastructure
{
  using System.Collections.Generic;

  public static class DictionaryEx
  {
    public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
    {
      TValue value;
      if (!dictionary.TryGetValue(key, out value))
      {
        return default(TValue);
      }

      return value;
    }
  }
}