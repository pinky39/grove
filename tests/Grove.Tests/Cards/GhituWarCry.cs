namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GhituWarCry
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Add30ToBear()
      {
        Battlefield(P1, "Ghitu War Cry", "Grizzly Bears", "Mountain", "Mountain", "Mountain");
        P2.Life = 5;
        
        RunGame(1);

        Equal(0, P2.Life);
      }  
    }
  }
}