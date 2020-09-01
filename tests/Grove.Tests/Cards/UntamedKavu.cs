namespace Grove.Tests.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class UntamedKavu
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void NoKicker()
      {
        var kavu = C("Untamed Kavu");
        
        Hand(P1, kavu);
        Battlefield(P1, "Forest", "Forest");

        RunGame(1);

        Equal(Zone.Battlefield, C(kavu).Zone);
        Equal(2, C(kavu).Power);        
      }

      [Fact]
      public void Kicker()
      {
        var kavu = C("Untamed Kavu");

        Hand(P1, kavu);
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(Zone.Battlefield, C(kavu).Zone);
        Equal(5, C(kavu).Power);
      }
    }
  }
}