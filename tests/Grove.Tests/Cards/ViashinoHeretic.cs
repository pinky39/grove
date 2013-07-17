namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ViashinoHeretic
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroySword()
      {
        var sword = C("Sword of Fire and Ice");

        Battlefield(P1, "Viashino Heretic", "Mountain", "Mountain");        
        Battlefield(P2, sword);

        RunGame(2);

        Equal(17, P2.Life);
        Equal(Zone.Graveyard, C(sword).Zone);
      }
    }
  }
}