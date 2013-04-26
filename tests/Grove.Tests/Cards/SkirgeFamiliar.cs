namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class SkirgeFamiliar
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayTitan()
      {
        var titan = C("Grave Titan");
        Hand(P1, titan, "Swamp", "Swamp", "Swamp");

        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Skirge Familiar");
        RunGame(1);

        Equal(Zone.Battlefield, C(titan).Zone);
      }
    }
  }
}