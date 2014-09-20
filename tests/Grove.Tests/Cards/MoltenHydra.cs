namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MoltenHydra
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal2Damage()
      {
        Battlefield(P1, "Molten Hydra", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Wall Of Blossoms");

        P2.Life = 2;
        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}