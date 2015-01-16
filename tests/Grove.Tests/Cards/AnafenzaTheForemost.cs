namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class AnafenzaTheForemost
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutCounteOnDragonAndExileBear()
      {
        var bear = C("Grizzly Bears");
        Battlefield(P1, "Shivan Dragon", "Anafenza, The Foremost");

        P2.Life = 7;
        Battlefield(P2, bear);

        RunGame(1);

        Equal(1, P2.Life);
        Equal(Zone.Exile, C(bear).Zone);
      }
    }
  }
}
