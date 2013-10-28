namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MantisEngine
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainsFlying()
      {
        Battlefield(P1, "Mantis Engine", "Island", "Island");
        Battlefield(P2, "Wall of Blossoms");
        P2.Life = 3;

        RunGame(1);

        Equal(0, P2.Life);
      }

      [Fact]
      public void GainsFirstStrike()
      {
        Battlefield(P1, "Mantis Engine", "Island", "Island");
        Battlefield(P2, "Radiant, Archangel");
        P2.Life = 3;

        RunGame(1);

        Equal(1, P2.Graveyard.Count);
        Equal(0, P1.Graveyard.Count);
      }
    }
  }
}