namespace Grove.Gameplay.Misc
{
  using System;

  public class Coin
  {
    private static readonly Random Rnd = new Random();
    
    public bool Flip()
    {
      var result = Rnd.Next(2);
      return result == 0;
    }
  }
}