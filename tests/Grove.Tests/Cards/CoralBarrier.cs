namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class CoralBarrier
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CreateToken()
      {
        Hand(P1, "Coral Barrier");
        Battlefield(P1, "Island", "Island", "Island");

        Battlefield(P2, "Grizzly Bears", "Island");

        RunGame(3);

        Equal(1, P1.Battlefield.Count(c => c.Is("Squid")));
        Equal(19, P2.Life);
      }
    }
  }
}