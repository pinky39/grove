namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ForgeDevil
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DevilDealsDamagesToYouAndToElf()
      {
        var devil = C("Forge Devil");
        var elves = C("Llanowar Elves");

        Battlefield(P1, "Mountain");        
        Hand(P1, devil);        
        Battlefield(P2, elves);

        RunGame(1);

        Equal(Zone.Battlefield, C(devil).Zone);
        Equal(Zone.Graveyard, C(elves).Zone);
        Equal(19, P1.Life);
      }
    }
  }
}
