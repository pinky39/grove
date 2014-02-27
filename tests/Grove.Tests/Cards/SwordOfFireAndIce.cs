namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SwordOfFireAndIce
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EquipAndAttack()
      {
        Battlefield(P1, C("Sword of Fire and Ice"), C("Grizzly Bears"), C("Forest"), C("Forest"));
        RunGame(maxTurnCount: 2);

        Equal(14, P2.Life);
      }

      [Fact]
      public void DoNotTriggerOnNonCombatDamage()
      {
        Battlefield(P1,
          C("Rumbling Slum").IsEquipedWith(C("Sword of Fire and Ice")));

        RunGame(maxTurnCount: 2);

        Equal(10, P2.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DestroyAttachedEquipment()
      {
        var sword = C("Sword of Fire and Ice");
        var slime = C("Acidic Slime");
        var bear = C("Grizzly Bears");

        Battlefield(P2, bear.IsEquipedWith(sword));
        Hand(P1, slime);

        Exec(
          At(Step.FirstMain)
            .Cast(slime)
            .Target(sword)
            .Verify(() =>
              {
                Equal(2, C(bear).Power);
                Equal(Zone.Graveyard, C(sword).Zone);
              })
          );
      }

      [Fact]
      public void Equip()
      {
        var sword = C("Sword of Fire and Ice");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");

        Battlefield(P1, sword, bear1);
        Battlefield(P2, bear2);

        Exec(
          At(Step.FirstMain)
            .Activate(sword, target: bear1)
            .Verify(() =>
              {
                True(C(bear1).HasAttachments);
                Equal(4, C(bear1).Power);
                Equal(4, C(bear1).Toughness);
                True(C(bear1).HasProtectionFrom(CardColor.Blue));
                True(C(bear1).HasProtectionFrom(CardColor.Red));
              }),
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear1),
          At(Step.CombatDamage)
            .Target(bear2),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(bear2).Zone);
                Equal(1, P1.Hand.Count());
              })
          );
      }
    }
  }
}