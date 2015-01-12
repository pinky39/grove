namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SuspensionField
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExileDragon()
      {
        Hand(P1, "Suspension Field");
        Battlefield(P1, "Plains", "Plains");

        Battlefield(P2, "Shivan Dragon");

        RunGame(1);

        Equal(0, P2.Battlefield.Count);
      }

      [Fact]
      public void ReturnDragonFromExile()
      {
        Hand(P1, "Suspension Field");
        Battlefield(P1, "Plains", "Plains");

        Hand(P2, "Naturalize");
        Battlefield(P2, "Shivan Dragon", "Forest", "Plains");

        RunGame(2);

        Equal(3, P2.Battlefield.Count);
        Equal(1, P1.Graveyard.Count);
      }
    }
  }
}
