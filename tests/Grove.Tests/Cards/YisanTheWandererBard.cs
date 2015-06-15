namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class YisanTheWandererBard
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutCounterOnYisanDontSearchForCard()
      {
        var yisan = C("Yisan, the Wanderer Bard");

        Battlefield(P1, yisan, "Forest", "Forest", "Forest");
        Battlefield(P2, "Wall of Denial");
        RunGame(2);

        Equal(1, C(yisan).CountersCount());
      }
    }
  }
}