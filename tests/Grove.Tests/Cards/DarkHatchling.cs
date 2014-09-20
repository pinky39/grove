namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DarkHatchling
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void NoLegalTargets()
      {
        var hatchling = C("Dark Hatchling");
        var force = C("Verdant Force");

        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Battlefield(P2, "Forest", "Forest", force);

        Hand(P1, hatchling);
        Hand(P2, "Vines Of Vastwood");

        RunGame(1);

        Equal(Zone.Battlefield, C(hatchling).Zone);
        Equal(Zone.Battlefield, C(force).Zone);
      }
    }
  }
}