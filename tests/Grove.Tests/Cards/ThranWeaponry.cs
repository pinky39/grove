namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;
    
  public class ThranWeaponry
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PumpAll()
      {
        Battlefield(P1, "Fleeting Image", "Fleeting Image", "Thran Weaponry", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, "Llanowar Elves");

        RunGame(3);

        Equal(4, P2.Life);
        Equal(6, P1.Battlefield.Lands.Count(x => !x.IsTapped));
      }

      [Fact]
      public void UntapWeaponry()
      {
        var bears = C("Grizzly Bears");
        
        Battlefield(P1, "Thran Weaponry", "Forest", "Forest", bears);
        Hand(P2, "Shivan Dragon");
        Battlefield(P2, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");

        RunGame(3);

        Equal(16, P2.Life);
        Equal(2, C(bears).Power);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void BearGetsBonus()
      {
        var weaponry = C("Thran Weaponry");
        var bears = C("Grizzly Bears");
        
        Hand(P1, bears);
        Battlefield(P1, weaponry, "Forest", "Forest", "Forest", "Forest");

        Exec(
          At(Step.FirstMain)
            .Activate(weaponry),
          At(Step.SecondMain)
            .Cast(bears)
            .Verify(() =>
              {
                Equal(4, C(bears).Power);
                Equal(4, C(bears).Toughness);
              })

        
        );
      }
    }
  }
}