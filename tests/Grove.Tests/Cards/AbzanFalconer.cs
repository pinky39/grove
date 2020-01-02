namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AbzanFalconer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveFlyingToCreaturesWithCounters()
      {
        var anthroplasm = C("Anthroplasm");
        Hand(P1, anthroplasm);
        Battlefield(P1, "Abzan Falconer", "Island", "Island", "Island", "Island");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");        

        RunGame(1);

        Assert.True(anthroplasm.Card.Has().Flying);        
      }
    }
  }
}
