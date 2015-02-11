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
        var bear = C("Grizzly Bears");
        Hand(P1, "Marked By Honor");        
        Battlefield(P1, bear, "Plains", "Plains", "Plains", "Plains");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
        False(C(bear).IsTapped);
      }
    }
  }
}
