namespace Grove.Gameplay.Misc
{
  using Infrastructure;

  [Copyable]
  public class Coin
  {
    private readonly RandomGenerator _randomGenerator;

    private Coin() {}

    public Coin(RandomGenerator randomGenerator)
    {
      _randomGenerator = randomGenerator;
    }

    public bool Flip()
    {
      var result = _randomGenerator.Next(2);
      return result == 0;
    }
  }
}