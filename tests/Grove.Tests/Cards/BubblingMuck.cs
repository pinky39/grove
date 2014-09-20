namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BubblingMuck
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastDragon()
      {
        var kite = C("Shivan Hellkite");
        Hand(P1, kite, "Bubbling Muck");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Swamp", "Swamp", "Swamp");

        RunGame(3);

        Equal(Zone.Battlefield, C(kite).Zone);
      }
    }
  }
}