namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BeastOfBurden
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PowerToughnessEqualToTotalCreatureCount()
      {
        var beast = C("Beast of Burden");
        
        Hand(P1, "Grizzly Bears", "Grizzly Bears");
        Battlefield(P1, "Wall of Blossoms", "Wall of Blossoms", "Forest", "Forest", "Forest", "Forest", beast);
        Battlefield(P2, "Mountain", "Wall of Denial");

        RunGame(1);

        Equal(6, C(beast).Power);
        Equal(6, C(beast).Toughness);
      }
    }
  }
}