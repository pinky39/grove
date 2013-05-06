namespace Grove.Gameplay.ManaHandling
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class MultiColorManaAmount : IManaAmount
  {
    private readonly List<SingleColorManaAmount> _groups = new List<SingleColorManaAmount>();

    public MultiColorManaAmount(Dictionary<ManaColor, int> colorCounts)
    {
      SingleColorManaAmount colorless = null;

      foreach (var colorCount in colorCounts)
      {
        var singleColor = new SingleColorManaAmount(colorCount.Key, colorCount.Value);
        Converted += colorCount.Value;

        if (singleColor.Color.IsColorless)
        {
          colorless = singleColor;
          continue;
        }

        _groups.Add(singleColor);
      }

      if (colorless != null)
      {
        _groups.Add(colorless);
      }
    }

    public int Converted { get; private set; }

    public IManaAmount Add(IManaAmount amount)
    {
      var dictionary = new Dictionary<ManaColor, int>();

      foreach (var singleColorAmount in amount)
      {
        dictionary[singleColorAmount.Color] = singleColorAmount.Count;
      }

      if (dictionary.Count == 0)
        return this;

      foreach (var singleColorAmount in _groups)
      {
        if (dictionary.ContainsKey(singleColorAmount.Color))
        {
          dictionary[singleColorAmount.Color] += singleColorAmount.Count;
          continue;
        }

        dictionary[singleColorAmount.Color] = singleColorAmount.Count;
      }

      return new MultiColorManaAmount(dictionary);
    }

    public IManaAmount Remove(IManaAmount amount)
    {
      var dictionary = _groups.ToDictionary(x => x.Color, x => x.Count);

      foreach (var singleColor in amount)
      {
        if (dictionary.ContainsKey(singleColor.Color))
        {
          if (dictionary[singleColor.Color] <= singleColor.Count)
          {
            dictionary.Remove(singleColor.Color);
          }
          else
          {
            dictionary[singleColor.Color] -= singleColor.Count;
          }
        }
      }

      if (dictionary.Count == 0)
        return Mana.Zero;

      if (dictionary.Count == 1)
        return new SingleColorManaAmount(dictionary.Keys.First(), dictionary.Values.First());

      return new MultiColorManaAmount(dictionary);
    }

    public IEnumerator<SingleColorManaAmount> GetEnumerator()
    {
      return _groups.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}