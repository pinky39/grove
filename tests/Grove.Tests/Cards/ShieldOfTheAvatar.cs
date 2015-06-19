namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ShieldOfTheAvatar
  {
    public class Predefined : PredefinedScenario 
    {
      [Fact]
      public void Prevent2Damage()
      {
        var bear = C("Grizzly Bears");
        var bolt = C("Lightning Bolt");
        
        Battlefield(P1, bear.IsEquipedWith("Shield of the Avatar"), "Llanowar Elves");
        Hand(P2, bolt);

        Exec(
          At(Step.FirstMain)
          .Cast(bolt, target: bear),
          At(Step.SecondMain)
          .Verify(() => Equal(1, C(bear).Damage)));

      }
    }
  }
}
