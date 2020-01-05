namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ArmamentCorps
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Give2CountersToCorps()
      {
        var corps = C("Armament Corps");
        Hand(P1, corps);
        Battlefield(P1, "Plains", "Swamp", "Forest", "Forest", "Forest");

        RunGame(1);

        Assert.Equal(6, C(corps).Power);
      }
    }
  }
}