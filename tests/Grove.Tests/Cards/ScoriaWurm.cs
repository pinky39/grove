namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ScoriaWurm
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ScoriaWurmReturnToHandEvantualy()
      {
        var wurm = C("Scoria Wurm");
        Battlefield(P1, wurm);

        RunGame(30);

        Equal(Zone.Hand, C(wurm).Zone);
      }
    }
  }
}