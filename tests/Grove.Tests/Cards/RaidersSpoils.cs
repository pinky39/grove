namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RaidersSpoils
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DrawCardForCombatDamage()
      {
        Hand(P1, "Raiders' Spoils");
        Battlefield(P1, "Swamp","Swamp","Swamp","Swamp", "Grizzly Bears", "Aven Skirmisher");

        RunGame(1);

        Equal(15, P2.Life);
        Equal(19, P1.Life);
        Equal(1, P1.Hand.Count);
      }
    }
  }
}
