namespace Grove.Tests.Cards
{
  using Core.Zones;
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

        RunGame(15);

        Equal(Zone.Hand, C(wurm).Zone);
      }
    }
  }
}