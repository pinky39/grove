namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  class RotfeasterMaggot
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainLifeEqualToDragonToughness()
      {
        Hand(P1, "Rotfeaster Maggot");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Graveyard(P1, "Shivan Dragon");

        Graveyard(P2, "Grizzly Bears");

        RunGame(1);

        Equal(25, P1.Life);
      }

      [Fact]
      public void CannotExileDragon()
      {
        Hand(P1, "Rotfeaster Maggot");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Graveyard(P1, "Shivan Dragon");

        Battlefield(P2, "Tormod's Crypt");

        RunGame(1);

      }
    }
  }
}
