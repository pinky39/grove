namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class PhyrexianTower
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastDragon()
      {
        var dragon = C("Shivan Dragon");
        Hand(P1, dragon);
        Battlefield(P1, "Mountain", "Mountain", "Swamp", "Phyrexian Tower", "Mountain", "Grizzly Bears");
        Battlefield(P2, "Trained Armodon");

        RunGame(3);

        Equal(Zone.Battlefield, C(dragon).Zone);
      }
    }
  }
}