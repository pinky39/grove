namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AbzanCharm
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Give2CountersToBear()
      {
        var charm = C("Abzan Charm");
        Hand(P1, charm);
        Battlefield(P1, "Plains", "Swamp", "Forest", "Grizzly Bears");
        P2.Life = 4;

        RunGame(1);

        Assert.Equal(0, P2.Life);
      }
    }
  }
}