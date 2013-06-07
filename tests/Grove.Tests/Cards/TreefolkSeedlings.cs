namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TreefolkSeedlings
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void VariableToughness()
      {
        var seedlings1 = C("Treefolk Seedlings");
        var seedlings2 = C("Treefolk Seedlings");

        Hand(P1, seedlings2);
        Battlefield(P1, seedlings1, "Forest");

        RunGame(1);
        
        Equal(1, C(seedlings1).Toughness);
        Equal(1, C(seedlings2).Toughness);
      }
    }
  }
}