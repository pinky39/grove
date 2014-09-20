namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PlanarCollapse
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyAllCreatures()
      {
        Battlefield(P1, "Grizzly Bears", "Planar Collapse", "Mountain");
        Battlefield(P2, "Shivan Dragon", "Shivan Dragon", "Shivan Dragon", "Swamp");

        RunGame(1);

        Equal(2, P1.Graveyard.Count);
        Equal(3, P2.Graveyard.Count);
      }
    }
  }
}