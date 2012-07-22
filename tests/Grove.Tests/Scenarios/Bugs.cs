namespace Grove.Tests.Scenarios
{
  using System.Linq;
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class Bugs
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BugDoAttackWithTrollsAndWildwood()
      {
        Hand(P1, "Birds of Paradise");
        Hand(P2, "Plains");

        Battlefield(P1, "Forest", "Sunpetal Grove", C("Troll Ascetic").IsEquipedWith("Sword of Feast and Famine"),
          "Stirring Wildwood", "Stirring Wildwood", "Troll Ascetic", "Forest", "Birds of Paradise");
        Battlefield(P2, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Wall of Reverence");

        P2.Life = 25;

        RunGame(1);

        Equal(19, P2.Life);
      }

      [Fact]
      public void BugDoNotTapLandsUselessly()
      {
        ScenarioCard mountain = C("Mountain");
        Battlefield(P1, C("Forest"), C("Forest"), mountain);
        Hand(P1, C("Llanowar Elves"));

        RunGame(maxTurnCount: 2);

        Equal(1, P1.Battlefield.Count(x => x.Name == "Forest" && x.IsTapped));
        False(mountain.IsTapped);
      }

      [Fact]
      public void BeastSacWithOpponentAbility()
      {
        Battlefield(P1, "Ravenous Baloth");
        Battlefield(P2, "Leatherback Baloth");

        RunGame(maxTurnCount: 2);
        Equal(1, P2.Battlefield.Count());
      }

      [Fact]
      public void BugGameHangsWhenEndOfGameIsReached1()
      {
        Battlefield(P1, C("Grizzly Bears"));
        P2.Life = 2;

        RunGame(maxTurnCount: 2);

        True(Game.IsFinished);
      }

      [Fact]
      public void BugGameHangsWhenEndOfGameIsReached2()
      {
        Battlefield(P2, C("Liliana's Specter"), C("Swamp"), C("Swamp"), C("Swamp"), C("Nantuko Shade"));
        Battlefield(P1, C("Mountain"), C("Forest"), C("Forest"));

        P1.Life = 1;

        RunGame(maxTurnCount: 2);

        True(Game.IsFinished);
      }

      [Fact]
      public void BugIncorrectStateCopy()
      {
        Battlefield(P1, C("Copperline Gorge"), C("Forest"),
          C("Leatherback Baloth").IsEquipedWith(C("Sword of Feast and Famine")), C("Ravenous Baloth"));

        RunGame(maxTurnCount: 1);
      }

      [Fact]
      public void BugLeakedCopyBasiliskCollar()
      {
        Hand(P2, "Martial Coup", "Deathless Angel", "Plains", "Deathless Angel");
        Hand(P1, "Stirring Wildwood", "Sunpetal Grove", "Plains", "Basilisk Collar");
        Battlefield(P2, "Plains", "Plains", "Plains", "Plains", "Hero of Bladehold");
        Battlefield(P1, "Razorverge Thicket", C("Student of Warfare").IsEnchantedWith("Rancor"), "Razorverge Thicket",
          "Plains", "Sword of Feast and Famine");

        RunGame(2);
      }        
    }

    public class PredifinedAi : PredefinedAiScenario
    {
      [Fact]
      public void BugBaneslayerAngelWontAttackAlone()
      {
        var angel = C("Baneslayer Angel");
        
        Hand(P1, "Copperline Gorge", "Thrun, the Last Troll");
        Hand(P2);
        Battlefield(P1, "Copperline Gorge", "Birds of Paradise", "Forest", "Copperline Gorge", "Rumbling Slum", "Rootbound Crag", "Forest", "Ravenous Baloth", "Forest", "Thrun, the Last Troll");
        Battlefield(P2, "Plains", "Plains", "White Knight", "Plains", "Glorious Anthem", "Plains", "White Knight", "Wall of Reverence", "Plains", "Plains", angel);       

        Exec(
          At(Step.SecondMain, turn: 2)
           .Verify(() =>
             {
               Equal(13, P1.Life);
               True(C(angel).IsTapped);
             })
        );
      }
      
      [Fact]
      public void BugOpalAcrolithCausesCombatException()
      {
        Hand(P1, "Pestilence", "Voice of Grace", "Corrupt", "Rune of Protection: Black");
        Hand(P2, "Day of Judgment", "Elesh Norn, Grand Cenobite", "Student of Warfare");
        Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Plains", "Plains", "Opal Acrolith", "Pestilence");
        Battlefield(P2, "Plains", "Plains", "Plains", "Plains", "Wall of Reverence", "Plains", "Wall of Reverence", "Hero of Bladehold", "Trip Noose");      
        
        Exec();
      }
      
      [Fact]
      public void BugSwordAngel()
      {
        ScenarioCard angel = C("Baneslayer Angel");
        ScenarioCard swords = C("Swords to Plowshares");

        Hand(P2, swords);

        Battlefield(P1, "Plains", "Plains", "White Knight", "Plains", "Plains",
          "Hero of Bladehold", "Plains", angel, "Plains", "White Knight", "Plains");
        Battlefield(P2, "Forest", "Sunpetal Grove", "Plains", "Llanowar Elves", "Plains",
          "Basilisk Collar", "Acidic Slime", "Wurmcoil Engine", "Thrun, the Last Troll", "Plains", "Troll Ascetic");

        P2.Life = 5;
        
        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(angel),
          At(Step.SecondMain, turn: 1)
            .Verify(() =>
              {               
                Equal(Zone.Graveyard, C(swords).Zone);
                Equal(Zone.Exile, C(angel).Zone);
                Equal(5, P2.Life);
              })
          );
          
      }

      [Fact]
      public void BugDoNotBlockStudentWithTroll()
      {
        ScenarioCard student = C("Student of Warfare");
        ScenarioCard troll = C("Troll Ascetic");

        Battlefield(P1, student);
        Battlefield(P2, troll);

        Exec(
          At(Step.FirstMain)
            .Activate(student)
            .Activate(student),
          At(Step.DeclareAttackers)
            .DeclareAttackers(student),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(17, P2.Life);
                Equals(Zone.Battlefield, troll);
              })
          );
      }

      [Fact]
      public void BugGameCrashesWhenBlockedAttackerIsKilled()
      {
        Hand(P1, "Angelic Page");
        Hand(P2, "Pestilence");
        
        Battlefield(P1, "Drifting Meadow", "Plains", "Plains", "Plains", 
          C("Voice of Grace").IsEnchantedWith("Brilliant Halo").IsEnchantedWith("Serra's Embrace"), "Plains", 
          C("Sanctum Custodian").IsEnchantedWith("Brilliant Halo"), "Dragon Blood", "Plains", "Plains");
        
        Battlefield(P2, "Drifting Meadow", "Swamp", "Rune of Protection: Black", "Swamp", "Disciple of Grace", 
          "Polluted Mire", "Plains", "Plains", "Urza's Armor", "Plains", "Worship", "Unworthy Dead", "Swamp", "Sanctum Guardian");       

        Exec();
      } 

      [Fact]
      public void BugRegenerateCombatDamage()
      {
        ScenarioCard thrun = C("Thrun, the Last Troll");
        ScenarioCard baloth = C("Ravenous Baloth");

        Battlefield(P1, baloth);
        Battlefield(P2, "Forest", "Forest", thrun);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(baloth),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(baloth).Zone);
                Equal(Zone.Battlefield, C(thrun).Zone);
              })
          );
      }

      [Fact]
      public void BugSearchWithoutResults()
      {
        ScenarioCard dragon = C("Shivan Dragon");
        ScenarioCard forest = C("Forest");

        Battlefield(P1, C("Forest").Tap(), forest, C("Mountain").Tap(), C("Mountain").Tap(),
          C("Mountain").Tap(), C("Llanowar Elves").Tap(), C("Llanowar Elves").Tap());
        Hand(P1, dragon);

        Battlefield(P2, C("Forest").Tap(), C("Mountain").Tap(), C("Grizzly Bears"), C("Forest"));
        Hand(P2, C("Elvish Warrior"), C("Grizzly Bears"), C("Order of the Sacred Bell"));

        Exec(
          At(Step.FirstMain)
            .Activate(forest) /* 1 mana must be left in pool after casting to trigger the bug*/
            .Cast(dragon)
            .Verify(() => Equal(Zone.Battlefield, C(dragon).Zone))
          );
      }

      [Fact]
      public void BugAttackingCausesEndOfGame()
      {
        bool gameEnded = true;

        ScenarioCard knight = C("White Knight");
        ScenarioCard student = C("Student of Warfare");
        ScenarioCard hero = C("Hero of Bladehold");

        Hand(P1, "Deathless Angel", "Baneslayer Angel", "Day of Judgment");
        Hand(P2, "Troll Ascetic");
        Battlefield(P1, "Plains", "Plains", "Plains", "Glorious Anthem", "Plains", knight, "Plains", student, hero);
        Battlefield(P2, "Razorverge Thicket", "Llanowar Elves", "Forest", "Troll Ascetic", "Plains", "Troll Ascetic",
          "Stirring Wildwood", "Plains", "Plains", "Wurmcoil Engine", "Birds of Paradise");

        Exec(
          At(Step.FirstMain)
            .Activate(student)
            .Activate(student),
          At(Step.DeclareAttackers)
            .DeclareAttackers(knight, student, hero),
          At(Step.SecondMain)
            .Verify(() => { gameEnded = false; })
          );

        False(gameEnded);
      }
    }
  }
}