namespace Grove.Tests.Cards
{
  using System.Linq;
  using Core.Details.Mana;
  using Grove.Core;
  using Infrastructure;
  using Xunit;

  public class SwordOfFeastAndFamine
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackWithSword()
      {         
        Hand(P2, "Forest", "Mountain");
        Battlefield(P1, C("Leatherback Baloth").IsEquipedWith(C("Sword of Feast and Famine")), C("Forest"));
        
        RunGame(maxTurnCount: 1);

        Equal(1, P2.Hand.Count);
      }
    }
    
    
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Equip()
      {
        var sword = C("Sword of Feast and Famine");
        var bear = C("Grizzly Bears");
        var forest = C("Forest");

        Battlefield(P1, sword, bear, forest);
        Hand(P2, "Stupor");

        Exec(
          At(Step.FirstMain)
            .Activate(forest) /* tap forest */
            .Activate(sword, target: bear)
            .Verify(() => {
              True(C(bear).HasAttachments);
              Equal(4, C(bear).Power);
              Equal(4, C(bear).Toughness);
              True(C(bear).HasProtectionFrom(ManaColors.Black));
              True(C(bear).HasProtectionFrom(ManaColors.Green));
            }),
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear),
          At(Step.SecondMain)
            .Verify(() => {
              False(forest.IsTapped);
              Equal(0, P2.Hand.Count());
            })
          );
      }

      [Fact]
      public void ReEquip()
      {
        var sword = C("Sword of Feast and Famine");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");


        Battlefield(P1, bear1.IsEquipedWith(sword), bear2);

        Exec(
          At(Step.Upkeep)
            .Verify(() => {
              True(C(bear1).HasAttachments);
              Equal(4, C(bear1).Power);
              False(C(bear2).HasAttachments);
            }),
          At(Step.FirstMain)
            .Activate(sword, target: bear2)
            .Verify(() => {
              True(C(bear2).HasAttachments);
              False(C(bear1).HasAttachments);
              Equal(4, C(bear2).Power);
              Equal(2, C(bear1).Power);
              True(C(bear2).HasProtectionFrom(ManaColors.Black));
              False(C(bear1).HasProtectionFrom(ManaColors.Black));
            })
          );
      }
    }
  }
}