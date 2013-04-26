namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class PendrellFlux
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantForce()
      {
        var force = C("Verdant Force");

        Hand(P1, "Pendrell Flux");
        Battlefield(P1, "Island", "Island");
        Battlefield(P2, force);

        RunGame(2);

        Equal(Zone.Graveyard, C(force).Zone);
      }
    }
  }
}