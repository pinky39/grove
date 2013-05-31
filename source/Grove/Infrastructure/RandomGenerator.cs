namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class RandomGenerator : ICopyable
  {
    private Random _random;

    private RandomGenerator() {}

    public RandomGenerator(int? seed = null)
    {
      Seed = seed ?? (int) DateTime.Now.Ticks;
      _random = new Random(Seed);
    }

    public int Seed { get; private set; }

    public void Copy(object original, CopyService copyService)
    {
      Seed = (int) DateTime.Now.Ticks;
      _random = new Random(Seed);
    }

    public int Next(int minValue, int maxValue)
    {
      return _random.Next(minValue, maxValue);
    }

    public int Next(int maxValue)
    {
      return _random.Next(maxValue);
    }

    public IList<int> GetRandomPermutation(int start, int count)
    {
      var range = Enumerable.Range(0, count);
      return ShuffleInPlace(range.ToArray());
    }

    private IList<int> ShuffleInPlace(IList<int> list)
    {
      // Fisher-Yates shuffle
      // http://en.wikipedia.org/wiki/Fisher-Yates
      for (var i = list.Count - 1; i >= 1; i--)
      {
        var j = Next(0, i);
        list.Swap(i, j);
      }

      return list;
    }
  }
}