namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Magnify
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackWithMagnify()
      {        
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Forest");
        Battlefield(P2, "Grizzly Bears");
        Hand(P1, "Magnify");
        P2.Life = 3;
        
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}