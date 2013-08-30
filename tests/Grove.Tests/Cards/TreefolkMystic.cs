namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class TreefolkMystic
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void DestroyAttackersAuras()
      {
        var bears = C("Grizzly Bears");
        Battlefield(P1, bears.IsEnchantedWith("Gaea's Embrace"));
        Battlefield(P2, "Treefolk Mystic");

        P2.Life = 5;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bears),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(bears).Zone))
          );
      }

      [Fact]
      public void DestroyBlockersAuras()
      {
        var bears = C("Grizzly Bears");
        var mystic = C("Treefolk Mystic");
        
        Battlefield(P1, mystic);
        Battlefield(P2, bears.IsEnchantedWith("Gaea's Embrace"));
        
        P2.Life = 2;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(mystic),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Graveyard, C(bears).Zone))
          );
      }
    }
  }
}