namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ShivanHellkite
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillScepterDeal1DamageToPlayer()
      {
        Battlefield(P1, "Shivan Hellkite", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Hypnotic Specter");

        RunGame(2);

        Equal(14, P2.Life);
      }
    }
  }
}