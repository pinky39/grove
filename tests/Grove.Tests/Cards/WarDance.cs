namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WarDance
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Add33ToBear()
      {
        Battlefield(P1, "War Dance", "Grizzly Bears");
        P2.Life = 9;
        RunGame(5);

        Equal(0, P2.Life);
      }
    }
  }
}