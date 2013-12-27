namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class SigilOfSleep
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BounceWurm()
      {
        var wurm = C("Yavimaya Wurm");
        
        Hand(P1, "Sigil of Sleep");
        Battlefield(P1, "Cloud of Faeries", "Island", "Island");
        
        Battlefield(P2, wurm);

        RunGame(1);

        Equal(Zone.Hand, C(wurm).Zone);        
      }
    }
  }
}