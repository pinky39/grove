namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class BarrinsCodex
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw3Cards()
      {
        var codex = C("Barrin's Codex");
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", codex);

        RunGame(6);

        Equal(5, P1.Hand.Count);
        Equal(Zone.Graveyard, C(codex).Zone);
      }
    }
  }
}