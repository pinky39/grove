namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class RuptureSpire
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Pay1()
      {
        var land = C("Mountain");
        var spire = C("Rupture Spire");

        Hand(P1, spire);
        Battlefield(P1, land);

        RunGame(1);

        Equal(Zone.Battlefield, C(spire).Zone);
        True(C(land).IsTapped);
      }
    }
  }
}