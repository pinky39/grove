namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class SoulSculptor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TurnDragonIntoEnchantment()
      {
        Battlefield(P1, "Shivan Dragon", "Soul Sculptor", "Plains", "Mountain");
        Battlefield(P2, "Shivan Dragon");

        P2.Life = 11;

        RunGame(3);

        Equal(0, P2.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void WhenAPlayerCastsCreatureTurnDragonBack()
      {
        var dragon = C("Shivan Dragon");
        var bear = C("Grizzly Bears");
        var sculptor = C("Soul Sculptor");
        
        Battlefield(P1, dragon);
        Hand(P1, bear);
        
        Battlefield(P2, sculptor);

        Exec(
          At(Step.FirstMain)
            .Activate(sculptor, target: dragon)
            .Verify(() => True(C(dragon).Is().Enchantment)),
          At(Step.SecondMain)
            .Cast(bear)
            .Verify(() => True(C(dragon).Is().Creature))            
        );
      }
    }
  }
}