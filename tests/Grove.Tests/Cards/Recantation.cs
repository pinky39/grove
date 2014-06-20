namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Recantation
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Bounce5Lands()
      {
        Battlefield(P1, C("Recantation").AddCounters(5, CounterType.Verse), "Island");
        Battlefield(P2, "Forest", "Mountain", "Forest", "Forest", "Forest");

        RunGame(2);

        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}