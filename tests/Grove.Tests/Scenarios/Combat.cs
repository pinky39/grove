namespace Grove.Tests.Scenarios
{
  using System.Linq;
  using Gameplay;
  using Grove.Infrastructure;
  using Infrastructure;
  using Xunit;

  public class Combat
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TryCastAt2NdMain()
      {
        var toolbox = C("Jhoira's Toolbox");
        
        Hand(P1, "Simian Grunts");
        Hand(P2, "Order of Yawgmoth");

        Battlefield(P1, "Forest", "Forest", "Forest");
        
        Battlefield(P2, toolbox, "Swamp", "Swamp", "Swamp", "Swamp");

        RunGame(2);

        Equal(Zone.Battlefield, C(toolbox).Zone);
      }
      
      [Fact]
      public void AttackWithAllCreatures()
      {
        Battlefield(P1, C("Grizzly Bears"), C("Llanowar Elves"), C("Elvish Warrior"));

        RunGame(maxTurnCount: 1);

        Equal(15, P2.Life);
      }

      [Fact]
      public void AttackWithDeathTouch()
      {
        Battlefield(P1, C("Vampire Nighthawk"), C("Vampire Nighthawk"));
        Battlefield(P2, C("Shivan Dragon"));

        RunGame(maxTurnCount: 1);

        Equal(24, P1.Life);
        Equal(16, P2.Life);
      }

      [Fact]
      public void DoNotAttack()
      {
        Battlefield(P1, C("Elvish Lyrist").IsEnchantedWith("Blanchwood Armor"), "Forest", "Forest", "Forest", "Treefolk Seedlings");
        Battlefield(P2, "Blanchwood Treefolk", C("Sanguine Guard").Tap());

        P2.Life = 10;

        RunGame(2);

        Equal(10, P2.Life);
      }


      [Fact]
      public void BugCreaturesWithFlyingCannotBeBlockedByCreaturesWithoutFlying()
      {
        Battlefield(P1, "Mountain", "Shivan Dragon");
        Battlefield(P2, C("Llanowar Behemoth"), C("Grizzly Bears"));

        P2.Life = 6;
        RunGame(maxTurnCount: 1);

        Assert.Equal(0, P2.Life);
      }

      [Fact]
      public void ChumpBlockWhenLifeIsLow()
      {
        Battlefield(P1, C("Llanowar Behemoth"));
        Battlefield(P2, C("Grizzly Bears"));
        P2.Life = 5;

        RunGame(maxTurnCount: 2);

        Equal(5, P2.Life);
        Equal(1, P1.Battlefield.Count());
        Equal(0, P2.Battlefield.Count());
      }

      [Fact]
      public void DoNotChumpBlockWhenLifeIsHigh()
      {
        Battlefield(P1, C("Llanowar Behemoth"));
        Battlefield(P2, C("Grizzly Bears"));
        RunGame(maxTurnCount: 2);

        Equal(1, P1.Battlefield.Count());
        Equal(1, P2.Battlefield.Count());
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void BugDestroyedBlockerDealsCombatDamage()
      {
        var bear1 = C("Grizzly bears");
        var bear2 = C("Grizzly bears");
        var armodon = C("Trained Armodon");
        var shock = C("Shock");

        Battlefield(P1, armodon);
        Hand(P2, shock);
        Battlefield(P2, bear1, bear2);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(armodon),
          At(Step.DeclareBlockers)
            .DeclareBlockers(armodon, bear1, armodon, bear2)
            .Cast(shock, bear1)
            .Verify(() => Equal(1, P1.Battlefield.Count())));

        Equal(0, P2.Battlefield.Count());
      }

      [Fact]
      public void BugEnchantedCreatureBlock()
      {
        var armodon1 = C("Trained Armodon");
        var armodon2 = C("Trained Armodon");
        var armodon3 = C("Trained Armodon");
        var armor = C("Blanchwood Armor");

        Battlefield(P1, armodon1.IsEnchantedWith(armor), "Forest", "Forest", "Forest");
        Battlefield(P2, armodon2, armodon3);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(armodon1),
          At(Step.DeclareBlockers)
            .DeclareBlockers(armodon1, armodon2, armodon1, armodon3),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(3, P1.Battlefield.Count());
                Equal(0, P2.Battlefield.Count());
              }));
      }

      [Fact]
      public void BugIncorrectDamageAssignment()
      {
        var armodon = C("Elvish warrior");
        var armor = C("Blanchwood Armor");
        var elves1 = C("Elvish warrior");
        var elves2 = C("Elvish warrior");

        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", armodon.IsEnchantedWith(armor));
        Battlefield(P2, elves1, elves2);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(armodon),
          At(Step.DeclareBlockers)
            .DeclareBlockers(armodon, elves1, armodon, elves2));

        Assert.Equal(0, P2.Battlefield.Count());
      }

      [Fact]
      public void DestroyAttacker()
      {
        var bear = C("Grizzly bears");
        var shock = C("Shock");

        Battlefield(P1, bear);
        Hand(P2, shock);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear)
            .Cast(shock, bear)
            .Verify(() =>
              True(Combat.Attackers.None())));
      }

      [Fact]
      public void GangBlock1()
      {
        var behemoth = C("Llanowar behemoth");
        var bear1 = C("Grizzly bears");
        var bear2 = C("Grizzly bears");

        Battlefield(P1, behemoth);
        Battlefield(P2, bear1, bear2);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(behemoth),
          At(Step.DeclareBlockers)
            .DeclareBlockers(behemoth, bear1, behemoth, bear2),
          At(Step.EndOfTurn)
            .Verify(() =>
              {
                Equal(0, P1.Battlefield.Count());
                Equal(0, P2.Battlefield.Count());
              }));
      }

      [Fact]
      public void GangBlock2()
      {
        var warrior1 = C("Elvish Warrior");
        var warrior2 = C("Elvish Warrior");
        var warrior3 = C("Elvish Warrior");
        var llanowar1 = C("Llanowar Elves");
        var llanowar2 = C("Llanowar Elves");
        var order1 = C("Order of the Sacred Bell");

        Battlefield(P1, order1, warrior1, warrior2);
        Battlefield(P2, warrior3, llanowar1, llanowar2);

        P2.Life = 4;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(order1, warrior1, warrior2),
          At(Step.DeclareBlockers)
            .DeclareBlockers(order1, llanowar1,
              warrior1, llanowar2, warrior1, warrior3)
          );

        Assert.Equal(2, P2.Life);
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact(Skip="Currently this always fails, since only one blocking strategy is considered.")]
      public void ShouldBlockWithUndeadInsteadOfReaver()
      {
        Battlefield(P2, "Unworthy Dead", "Unworthy Dead", "Flesh Reaver", "Swamp", "Swamp");
        var hippo = C("Bull Hippo");
        Battlefield(P1, hippo);

        P2.Life = 3;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(hippo));

        Equal(3, P2.Life);
      }
      
      [Fact]
      public void PlayerShouldAttack()
      {
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Island", "Lightning Dragon", "Mountain");
        Battlefield(P2, "Plains", "Plains", "Plains", "Angelic Wall", "Plains", "Plains", "Hero of Bladehold",
          "Baneslayer Angel", "Hero of Bladehold");

        P1.Life = 8;
        P2.Life = 25;

        RunGame(6);

        // game should not run longer than 2 turns
        Equal(2, Game.Turn.TurnCount);
      }

      [Fact]
      public void BugPowerIncrease()
      {
        var bear = C("Grizzly Bears");
        var warrior = C("Elvish Warrior");
        var vines = C("Vines of Vastwood");

        Hand(P1, vines);
        Battlefield(P1, bear);
        Battlefield(P2, warrior);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear),
          At(Step.DeclareBlockers)
            .Cast(vines, target: bear, index: 1),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(warrior).Zone);
                Equal(Zone.Battlefield, C(bear).Zone);
              })
          );
      }

      [Fact]
      public void BugNoAttackWhenPlayingBaloth()
      {
        var baloth = C("Ravenous Baloth");

        Hand(P1, C("Leatherback Baloth"), C("Burst Lightning"), C("Forest"), C("Rumbling Slum"), C("Lightning Bolt"),
          C("Sword of Fire and Ice"), C("Rootbound Crag"), C("Forest"));
        Hand(P2, C("Vigor"), C("Lightning Bolt"), C("Volcanic Fallout"), baloth, C("Forest"));

        Battlefield(P1, C("Raging Ravine"), C("Forest"));
        Battlefield(P2, C("Copperline Gorge"), C("Llanowar Elves"), C("Forest"), C("Leatherback Baloth"));

        Exec(
          At(Step.SecondMain, turn: 2)
            .Verify(() =>
              {
                Equal(13, P1.Life);
                Equal(Zone.Battlefield, C(baloth).Zone);
              })
          );
      }
    }
  }
}