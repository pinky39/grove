namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CitanulFlute
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void FetchBears()
      {
        var bears = C("Grizzly Bears");
        Library(P1, "Forest", "Forest", bears, "Shivan Dragon", "Forest");
        Battlefield(P1, "Citanul Flute", "Forest", "Forest");

        RunGame(2);

        Equal(Zone.Hand, C(bears).Zone);
        
      }
    }
  }
}