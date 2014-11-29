namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SiegeWurm
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void CastWithConvoke()
      {
        var wurm = C("Siege Wurm");
        Hand(P1, wurm);
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        Battlefield(P2, "Ravenous Baloth");

        RunGame(1);

        Equal(Zone.Battlefield, C(wurm).Zone);
      }  
    }        
  }
}