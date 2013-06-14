namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class VernalBloom
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastForce()
      {
        var force1 = C("Verdant Force");
        var force2 = C("Verdant Force");
        
        Hand(P1, force1);
        Hand(P2, force2);
        
        Battlefield(P1, "Vernal Bloom", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Forest", "Forest", "Forest", "Forest");

        RunGame(2);

        Equal(Zone.Battlefield, C(force1).Zone);
        Equal(Zone.Battlefield, C(force2).Zone);
      }
    }
  }
}