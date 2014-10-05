namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class MarkedByHonor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveVigilanceAnd22()
      {
        Hand(P1, "Marked By Honor");
        Battlefield(P1, "Grizzly Bears", "Plains", "Plains", "Plains", "Plains");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
        True(P1.Battlefield.Creatures.All(c => !c.IsTapped));
      }
    }
  }
}
