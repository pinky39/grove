namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class FirstResponse
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Player2GetsAToken()
      {
        Battlefield(P1, "Shivan Dragon");
        Battlefield(P2, "First Response");

        RunGame(2);

        Equal(15, P2.Life);
        Equal(1, P2.Battlefield.Count(x => x.Is().Token));
      }
    }
  }
}