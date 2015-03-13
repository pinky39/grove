namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Dismember
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastSpellForPhyrexianCost()
      {
        Battlefield(P1, "Shivan Dragon");

        P2.Life = 5;
        Hand(P2, "Dismember");
        Battlefield(P2, "Swamp");

        RunGame(1);

        Equal(0, P2.Hand.Count);
        Equal(1, P2.Life);
      }

      [Fact]
      public void CastSpellForPhyrexianCost2()
      {
        Battlefield(P1, "Shivan Dragon");

        P2.Life = 5;
        Hand(P2, "Dismember");
        Battlefield(P2, "Swamp", "Swamp");

        RunGame(1);

        Equal(0, P2.Hand.Count);
        Equal(3, P2.Life);
      }
    }
  }
}
