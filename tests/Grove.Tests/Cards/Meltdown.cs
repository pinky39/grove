namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class Meltdown
  {
    public class Ai : AiScenario
    {

      [Fact]
      public void DestroyEachWith2ConvertedCost2OrLess()
      {
        Battlefield(P1, "Trained Armodon", "Trained Armodon", "Cathodion", 
          "Mountain", "Mountain", "Mountain", "Mountain");
        Hand(P1, "Meltdown");
        
        Battlefield(P2, "Trip Noose", "Trip Noose", "Forest", "Forest", "Forest", "Forest");

        RunGame(1);

        Equal(0, P2.Battlefield.Count(x => x.Is().Artifact));
        Equal(1, P1.Battlefield.Count(x => x.Is().Artifact));
      }
    }
  }
}