namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class Masticore
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DiscardCardKillBearAttack()
      {
        Hand(P1, "Island");
        Battlefield(P1, "Masticore", "Island", "Island", "Island", "Island");
        Battlefield(P2, "Grizzly Bears");
        P2.Life = 4;

        RunGame(1);

        Equal(1, P1.Graveyard.Count(c => c.Is().Land));
        Equal(0, P2.Life);
      }
    }
  }
}