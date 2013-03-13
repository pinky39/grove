namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class GildedDrake
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void NoValidTarget()
      {
        var drake = C("Gilded Drake");
        var guma = C("Guma");
        
        Hand(P1, drake);        
        Battlefield(P2, guma);

        Exec(
          At(Step.FirstMain)
            .Cast(drake)
            .NoValidTarget(),            
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(drake).Zone);
              })
        );

      }
    }
    
    public class Ai : AiScenario
    {            
      [Fact]
      public void ExchangeForForce()
      {
        var drake = C("Gilded Drake");
        var force = C("Verdant Force");

        Hand(P1, drake);
        Battlefield(P1, "Island", "Island");
        Battlefield(P2, force);

        RunGame(1);

        Equal(P1, C(force).Controller);
        Equal(P2, C(drake).Controller);
      }


      [Fact]
      public void ProtectForce()
      {
        var drake = C("Gilded Drake");
        var force = C("Verdant Force");

        Hand(P1, drake);
        Hand(P2, "Vines of Vastwood");
        
        Battlefield(P1, "Island", "Island");
        Battlefield(P2, force, "Forest", "Forest");

        RunGame(1);

        Equal(P2, C(force).Controller);        
        Equal(Zone.Graveyard, C(drake).Zone);
      }
    }
  }
}