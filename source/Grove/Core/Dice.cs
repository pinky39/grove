namespace Grove.Core
{
  using System;

  public class Dice
  {
    public const int NumOfSides = 42;
    private static readonly Random Rnd = new Random();
    public int LastResult { get; private set; }

    public void Roll()
    {
      LastResult = Rnd.Next(1, NumOfSides);
    }
  }
}