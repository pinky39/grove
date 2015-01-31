namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class UndergrowthScavenger
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayScavengerWithThreeCountersOnIt()
      {
        var scavenger = C("Undergrowth Scavenger");
        Hand(P1, scavenger);
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest");
        Graveyard(P1, "Shivan Dragon");

        Graveyard(P2, "Abzan Beastmaster", "Shivan Dragon");

        RunGame(1);

        Equal(Zone.Battlefield, C(scavenger).Zone);
        Equal(3, C(scavenger).Power);
      }
    }
  }
}
