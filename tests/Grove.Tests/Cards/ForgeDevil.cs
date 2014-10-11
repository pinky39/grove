namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ForgeDevil
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DevilDealsDamagesToYouAndCreature()
      {
        Battlefield(P1, "Mountain");
        Hand(P1, "Forge Devil");

        Battlefield(P2, "Forge Devil");

        RunGame(1);

        Equal(0, P1.Hand.Count);
        Equal(0, P2.Battlefield.Count);
        Equal(19, P1.Life);
      }
    }
  }
}
