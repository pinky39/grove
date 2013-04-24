namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class DarkRitual
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastDragon()
      {
        var dragon = C("Shivan Dragon");
        Hand(P1, "Dark Ritual", dragon);
        Battlefield(P1, "Mountain", "Mountain", "Swamp", "Mountain");

        RunGame(1);

        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }
  }
}