namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class EnsoulArtifact
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantWithEnsoulArtifact()
      {
        Hand(P1, "Ensoul Artifact");
        Battlefield(P1, "Profane Memento", "Island", "Island");

        RunGame(1);

        Equal(15, P2.Life());
      }
    }
  }
}