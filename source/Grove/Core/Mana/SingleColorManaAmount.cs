namespace Grove
{
  using System.Collections;
  using System.Collections.Generic;

  public class SingleColorManaAmount : IManaAmount
  {
    public SingleColorManaAmount(ManaColor color, int count)
    {
      Color = color;
      Count = count;
    }

    public ManaColor Color { get; private set; }
    public int Count { get; private set; }

    public IEnumerator<SingleColorManaAmount> GetEnumerator()
    {
      yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Converted
    {
      get
      {
        return Count;
      }
    }

    public int Generic 
    {
      get
      {
        return Color.IsColorless ? Count : 0;
      }
    }

    public HashSet<int> Colors { get { return new HashSet<int>(Color.Indices); } }

    public IManaAmount Add(IManaAmount amount)
    {
      var dictionary = new Dictionary<ManaColor, int>();

      foreach (var singleColorAmount in amount)
      {
        dictionary[singleColorAmount.Color] = singleColorAmount.Count;
      }

      if (dictionary.Count == 0)
        return this;

      if (dictionary.ContainsKey(Color))
      {
        dictionary[Color] += Count;
      }
      else
      {
        dictionary[Color] = Count;
      }

      if (dictionary.Count == 1)
        return new SingleColorManaAmount(Color, dictionary[Color]);

      return new MultiColorManaAmount(dictionary);
    }

    public IManaAmount Remove(IManaAmount amount)
    {
      var count = Count;

      foreach (var singleColor in amount)
      {
        if (singleColor.Color == Color)
        {
          count -= singleColor.Count;
        }
      }

      if (count > 0)
        return new SingleColorManaAmount(Color, count);

      return Mana.Zero;
    }
  }
}