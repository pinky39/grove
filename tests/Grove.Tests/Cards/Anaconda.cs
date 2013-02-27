namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Anaconda
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Swampwalk()
      {
        Battlefield(P1, "Anaconda");
        Battlefield(P2, "Grizzly Bears", "Swamp");

        P2.Life = 3;
        
        RunGame(1);
        Equal(0, P2.Life);
      }
    }
  }
}