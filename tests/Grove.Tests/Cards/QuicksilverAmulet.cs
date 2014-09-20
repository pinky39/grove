namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class QuicksilverAmulet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutDragonIntoPlay()
      {
        var hellkite = C("Shivan Hellkite");
        Hand(P1, hellkite, "Forest", "Grizzly Bears");
        Battlefield(P1, "Quicksilver Amulet", "Forest", "Forest", "Forest", "Forest");

        RunGame(2);

        Equal(Zone.Battlefield, C(hellkite).Zone);
      }
    }
  }
}