namespace Grove.Tests.Cards
{
  using System.Linq;
  using Core;
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class WildDogs
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void ChangeControl()
      {
        var dogs = C("Wild Dogs");
                        
        Battlefield(P1, dogs);

        P1.Life = 19;
        P2.Life = 20;

        Exec(
          At(Step.FirstMain)
            .Verify(() =>
              {
                Equal(1, P2.Battlefield.Count());
                Equal(0, P1.Battlefield.Count());
                Equal(P2, C(dogs).Controller);
              })
        );
      }

      [Fact]
      public void AttachmentsAlsoChangeBattlefield()
      {
        var dogs = C("Wild Dogs");
        var bear = C("Grizzly Bears");
        var sword = C("Sword of Fire and Ice");

        Battlefield(P1, dogs.IsEquipedWith(sword), bear);
        
        P1.Life = 19;
        P2.Life = 20;

         Exec(
          At(Step.FirstMain)
            .Verify(() =>
              {
                Equal(2, P2.Battlefield.Count());
                Equal(4, C(dogs).Power);
                Equal(1, P1.Battlefield.Count());                
              })
            .Activate(sword, target: bear)
            .Verify(() =>
              {
                Equal(1, P2.Battlefield.Count());
                Equal(2, C(dogs).Power);
                Equal(2, P1.Battlefield.Count());
              })
        );
      }
    }
  }
}