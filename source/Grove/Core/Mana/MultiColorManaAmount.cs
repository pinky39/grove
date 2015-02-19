namespace Grove
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class MultiColorManaAmount : ManaAmount, IEnumerable
  {
    private readonly List<SingleColorManaAmount> _groups = new List<SingleColorManaAmount>();
    private readonly int _converted;
    private readonly int _generic;

    public MultiColorManaAmount(Dictionary<ManaColor, int> colorCounts)
    {
      SingleColorManaAmount colorless = null;

      _generic = 0;

      foreach (var colorCount in colorCounts)
      {
        var singleColor = new SingleColorManaAmount(colorCount.Key, colorCount.Value);
        _converted += colorCount.Value;

        if (singleColor.Color.IsColorless)
        {
          colorless = singleColor;

          _generic += colorless.Count;

          continue;
        }

        _groups.Add(singleColor);
      }

      if (colorless != null)
      {
        _groups.Add(colorless);
      }
    }

    public override int Converted { get { return _converted; } }
    public override int Generic { get { return _generic; }}

    public override HashSet<int> Colors
    {
      get
      {
        var indices = new HashSet<int>();

        foreach (var amount in _groups)
        {
          indices.UnionWith(amount.Colors);
        }
        return indices;
      }
    }

    public override ManaAmount Add(ManaAmount amount)
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

    public override ManaAmount Remove(ManaAmount amount)
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

    public override IEnumerator<SingleColorManaAmount> GetEnumerator()
    {
      return _groups.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}