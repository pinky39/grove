namespace Grove.Gameplay.Misc
{
  using Infrastructure;

  [Copyable]
  public class Dice
  {
    public const int NumOfSides = 42;
    private readonly RandomGenerator _randomGenerator;

    private Dice() {}

    public Dice(RandomGenerator randomGenerator)
    {
      _randomGenerator = randomGenerator;
    }

    public int Roll()
    {
      return _randomGenerator.Next(1, NumOfSides);
    }
  }
}