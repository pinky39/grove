namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class GaeasBounty
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void FetchForests()
      {
        Library(P1, "Forest", "Forest");
        Hand(P1, "Gaea's Bounty");
        Battlefield(P1, "Forest", "Mountain", "Mountain");

        RunGame(1);

        Equal(2, P1.Battlefield.Count(x => x.Is("forest")));
        Equal(1, P1.Hand.Count(x => x.Is("forest")));
      }
    }
  }
}