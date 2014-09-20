namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Disappear
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnDragon()
      {
        Hand(P1, "Disappear");
        Battlefield(P1, "Grizzly Bears", "Island", "Island", "Island", "Island", "Island");
        Battlefield(P2, "Shivan Dragon");
        P2.Life = 2;
        
        RunGame(1);
        
        Equal(0, P2.Life);        
      }
    }
  }
}