namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SeasonedMarshal
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackForKill()
      {
        Battlefield(P1, "Seasoned Marshal", "Seasoned Marshal");
        Battlefield(P2, "Shivan Dragon", "Grizzly Bears");

        P2.Life = 4;
        P1.Life = 5;

        RunGame(2);

        Equal(5, P1.Life);
        Equal(0, P2.Life);
      }
    }
  }
}