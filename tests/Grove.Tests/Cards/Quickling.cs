namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class Quickling
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacrificeQuickling()
      {
        var quickling = C("Quickling");
        Hand(P1, quickling);
        Battlefield(P1, "Island", "Island", "Fugitive Wizard");

        RunGame(1);

        Battlefield(P2, "Wall of Frost");

        Equal(Zone.Battlefield, C(quickling).Zone);
      }
    }
  }
}
