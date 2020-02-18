namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class EliteScaleguard
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BolsterAndTap()
      {
        var bear = C("Grizzly Bears");
        
        Hand(P1, "Elite Scaleguard");
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", bear);        
        Battlefield(P2, "Grizzly Bears");

        P2.Life = 4;
        RunGame(1);

        Equal(4, C(bear).Power);
        Equal(0, P2.Life);
      }
    }
  }
}