namespace Grove.Infrastructure
{
  using System;

  public class RandomGenerator : ICopyable
  {
    private Random _random;
    public int Seed { get; private set; }

    public RandomGenerator(int? seed = null)
    {
      Seed = seed ?? (int)DateTime.Now.Ticks;
      _random = new Random(Seed);
    }

    public int Next(int minValue, int maxValue)
    {
      return _random.Next(minValue, maxValue);
    }

    public void Copy(object original, CopyService copyService)
    {      
      Seed = (int) DateTime.Now.Ticks;
      _random = new Random(Seed);
    }
  }
}