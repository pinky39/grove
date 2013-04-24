namespace Grove.Tests.Scenarios
{
  using System.Linq;
  using Core;
  using Gameplay.States;
  using Gameplay.Zones;
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
        var mountain = C("Mountain");
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
      public void BugGameHangs()
      {           
        Hand(P1, "Go for the Throat", "Attunement", "Mana Leak", "Grave Titan");
        Hand(P2, "Mountain", "Rescind");
        Battlefield(P1, "Swamp", "Island", "Swamp", "Swamp", "Diabolic Servitude", "Swamp", "Drifting Djinn", "Swamp", "Liliana's Specter");
        Battlefield(P2, "Remote Isle", "Island", "Island", "Mountain", "Mountain", "Mountain", "Mountain");        

        RunGame(2);
      
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
        Battlefield(P1, "Copperline Gorge", "Birds of Paradise", "Forest", "Copperline Gorge", "Rumbling Slum",
          "Rootbound Crag", "Forest", "Ravenous Baloth", "Forest", "Thrun, the Last Troll");
        Battlefield(P2, "Plains", "Plains", "White Knight", "Plains", "Glorious Anthem", "Plains", "White Knight",
          "Wall of Reverence", "Plains", "Plains", angel);

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
      public void BugSwordAngel()
      {
        var angel = C("Baneslayer Angel");
        var swords = C("Swords to Plowshares");

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
        var student = C("Student of Warfare");
        var troll = C("Troll Ascetic");

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
          "Polluted Mire", "Plains", "Plains", "Urza's Armor", "Plains", "Worship", "Unworthy Dead", "Swamp",
          "Sanctum Guardian");

        Exec();
      }

      [Fact]
      public void BugRegenerateCombatDamage()
      {
        var thrun = C("Thrun, the Last Troll");
        var baloth = C("Ravenous Baloth");

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
        var dragon = C("Shivan Dragon");
        var forest = C("Forest");

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
        var gameEnded = true;

        var knight = C("White Knight");
        var student = C("Student of Warfare");
        var hero = C("Hero of Bladehold");

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

      [Fact]
      public void BugLeakedCopyWithShivsEmbrace()
      {
        var anaconda = C("Anaconda");
        var shivs = C("Shiv's Embrace");
        var bear = C("Grizzly Bears");
        var cradle = C("Cradle Guard");


        Battlefield(P1, anaconda.IsEnchantedWith(shivs), "Mountain", "Mountain", "Forest", "Mountain");
        Hand(P2, cradle);
        Battlefield(P2, bear, "Forest", "Forest", "Mountain", "Forest");

        RunGame(2);
      }

      [Fact]
      public void BugSymbiosisIncorectAiTargetAssignement()
      {
        Hand(P1, "Humble", "Remote Isle", "Confiscate");
        Hand(P2, "Symbiosis", "Hush", "Thundering Giant", "Torch Song", "Hidden Ancients");
        
        Battlefield(P1, "Island", "Swamp", "Island", "Swamp", "Swamp", "Plains", "Swamp",
          C("Sandbar Serpent").IsTrackedBy("Diabolic Servitude"));
        
        Battlefield(P2, "Mountain", "Slippery Karst", "Thran Turbine", "Smoldering Crater", "Goblin War Buggy", "Forest",
          "Cradle Guard", "Goblin War Buggy");

        RunGame(2);
      }
      
      [Fact]
      public void BugAnnulValidator()
      {
        Hand(P2, "Copper Gnomes", "Annul");
        Battlefield(P2, "Island", "Forest", "Forest", "Island");        

        RunGame(2);
      }    
      
      [Fact]
      public void BugCitanulHierophantsManaSourcesAddRemove()
      {        
        Hand(P1, "Bulwark", "Congregate", "Disorder", "Cloak of Mists", "Creeping Tar Pit");
        Hand(P2, "Plains");
        Battlefield(P1, "Creeping Tar Pit", "Island", "Island", "Drowned Catacomb", "Creeping Tar Pit", "Mountain", "Back to Basics", "Drowned Catacomb", "Plains", "Forest", "Citanul Hierophants");
        Battlefield(P2, "Razorverge Thicket", "Forest", "Drowned Catacomb", "Argothian Enchantress",  "Island", "Chimeric Staff", "Drowned Catacomb", "Drowned Catacomb", "Island", "Swamp", "Plains", "Swamp", "Forest");        

        RunGame(2);
      }    

      [Fact]
      public void BugContiniousEffectFromCitanulAppliedTwiceToBarrin()
      {
        Hand(P1, "Annul", "Back to Basics", "Crater Hellion", "Argothian Elder", "Antagonism", "Cave Tiger", "Bedlam");
        Hand(P2, "Discordant Dirge", "Annul", "Barrin, Master Wizard", "Crater Hellion");
        Battlefield(P1, "Creeping Tar Pit", "Mountain", "Rootbound Crag", "Swamp", "Swamp", "Darkest Hour", "Creeping Tar Pit");
        Battlefield(P2, "Razorverge Thicket", "Creeping Tar Pit", "Mountain", "Disruptive Student", "Island", "Back to Basics", "Crystal Chimes", "Swamp", "Phyrexian Ghoul", "Forest", "Citanul Hierophants", "Island");     

        RunGame(2);
      }

      [Fact]
      public void BugDarkRitualCheckSpellsWithNoCastingCost()
      {
        Hand(P1, "Argothian Elder", "Copper Gnomes", "Dark Hatchling", "Forest", "Island", "Dark Ritual", "Disorder");
        Hand(P2, "Congregate", "Dark Ritual", "Island", "Dark Ritual", "Dark Ritual", "Cloak of Mists", "Crater Hellion");
        Battlefield(P1, "Creeping Tar Pit");
        Battlefield(P2, "Creeping Tar Pit");    

        RunGame(2);
      }

      [Fact]
      public void GameHangsBecauseOfABuginManaPool()
      {        
        Hand(P1, "Lightning Dragon");
        Hand(P2, "Swords to Plowshares", "Plains", "Plains", "Plains", "Trip Noose");
        Battlefield(P1, "Forest", C("Copperline Gorge").IsEnchantedWith("Fertile Ground"), "Mountain", "Mountain", "Forest", C("Lightning Dragon").IsEnchantedWith("Rancor").IsEnchantedWith("Rancor"));
        Battlefield(P2, "Plains", "Plains", "Plains", "Glorious Anthem", "Plains", "Glorious Anthem", "Plains", "Baneslayer Angel", "Plains");

        P1.Life = 12;
        P2.Life = 17;

        EnableLogging();

        RunGame(3);
      }   
      
    }
  }
}