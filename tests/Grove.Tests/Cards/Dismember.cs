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
        var dragon = C("Shivan Dragon");
        Battlefield(P1, dragon);

        P2.Life = 5;
        Hand(P2, "Dismember");
        Battlefield(P2, "Swamp");

        RunGame(1);

        Equal(1, P2.Life);
        Equal(Zone.Graveyard, C(dragon).Zone);
      }    
    }
  }
}
