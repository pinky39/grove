namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DiseaseCarriers
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacrificeCarriersAndAttack()
      {
        Battlefield(P1, "Phyrexian Plaguelord", "Disease Carriers");
        Battlefield(P2, "Fog Bank", "Llanowar elves");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
        Equal(2, P2.Graveyard.Count);
      }
    }
  }
}