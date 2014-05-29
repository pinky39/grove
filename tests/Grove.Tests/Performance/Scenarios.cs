namespace Grove.Tests.Performance
{
  using AI;
  using Infrastructure;
  using Xunit;

  public class Scenarios : AiScenario
  {
    public Scenarios() {}

    public Scenarios(SearchParameters searchParameters) : base(searchParameters) {}

    public void Num01()
    {
      Hand(P1, C("Llanowar Elves"), C("Volcanic hammer"), C("Forest"));
      Battlefield(P1, C("Grizzly Bears"), C("Forest"), C("Mountain"), C("Llanowar Behemoth"));
      Battlefield(P2, C("Forest"), C("Forest"), C("Forest"), C("Mountain"), C("Grizzly Bears"),
        C("Llanowar Behemoth"));

      RunGame(maxTurnCount: 2);
    }

    public void Num02()
    {
      Hand(P1, C("Llanowar Elves"), C("Volcanic hammer"), C("Forest"), C("Mountain"));
      Battlefield(P1, C("Grizzly Bears"), C("Forest"), C("Mountain"), C("Mountain"), C("Shivan Dragon"));
      Battlefield(P2, C("Swamp"), C("Swamp"), C("Swamp"), C("Swamp"), C("Swamp"), "Drana, Kalastria Bloodchief");

      RunGame(maxTurnCount: 2);
    }

    public void Num03()
    {
      Battlefield(P2, C("Swamp"), C("Swamp"), C("Swamp"), C("Swamp"), C("Nantuko shade"),
        C("Drana, Kalastria Bloodchief"));
      Battlefield(P1, C("Llanowar behemoth"), C("Elvish Warrior"), C("Elvish Warrior"), C("Trained Armodon"));

      RunGame(maxTurnCount: 2);
    }

    public void Num04()
    {
      Hand(P1, C("Swamp"), C("Stupor"), C("Nantuko Shade"), C("Cruel Edict"));
      Hand(P2, C("Mountain"), C("Raging Ravine"), C("Thrun, the Last Troll"));

      Battlefield(P1, C("Swamp"), C("Swamp"), C("Swamp"), C("Nantuko Shade"));
      Battlefield(P2, C("Raging Ravine"), C("Rootbound Crag"), C("Mountain"), C("Fires of Yavimaya"), C("Rumbling Slum"),
        C("Thrun, the Last Troll"));

      RunGame(maxTurnCount: 1);
    }

    public void Num05()
    {
      Hand(P1, C("Volcanic Fallout"), C("Volcanic Fallout"), C("Vigor"), C("Rumbling Slum"));
      Hand(P2, C("Lightning Bolt"), C("Burst Lightning"), C("Fires of Yavimaya"));
      Battlefield(P1, C("Copperline Gorge"), C("Forest"), C("Forest"),
        C("Leatherback Baloth").IsEquipedWith(C("Sword of Feast and Famine")), C("Forest"), C("Ravenous Baloth"),
        C("Leatherback Baloth"));
      Battlefield(P2, C("Copperline Gorge"), C("Forest"), C("Rootbound Crag"), C("Rootbound Crag"), C("Ravenous Baloth"),
        C("Raging Ravine"), C("Leatherback Baloth"), C("Copperline Gorge"));

      RunGame(maxTurnCount: 1);
    }

    public void Num06()
    {
      Hand(P1, "Grasp of Darkness", "Wurmcoil Engine");
      Battlefield(P2, "Forest", "Rootbound Crag", "Forest", "Forest", "Fires of Yavimaya", "Forest", "Rootbound Crag",
        "Ravenous Baloth", "Raging Ravine");
      Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Nantuko Shade", "Swamp");

      RunGame(maxTurnCount: 1);
    }

    public void Num07()
    {
      Hand(P1, "Sift", "Darksteel Gargoyle", "Divination", "Mana Leak", "Darksteel Gargoyle", "Blaze", "Mana Leak");
      Hand(P2, "Vines of Vastwood", "Volcanic Fallout", "Volcanic Fallout");
      Battlefield(P1, "Rupture Spire", "Island", "Wall of Denial", "Mountain", "Trip Noose", "Steam Vents", "Mountain",
        "Steam Vents", "Seal of Fire");
      Battlefield(P2, "Rootbound Crag", "Llanowar Elves", "Copperline Gorge", "Forest", "Ravenous Baloth", "Forest",
        "Acidic Slime", "Raging Ravine", "Forest", "Rootbound Crag", "Forest");

      RunGame(maxTurnCount: 2);
    }

    public void Num08()
    {
      Hand(P1, "Marsh Casualties", "Marsh Casualties", "Cruel Edict", "Marsh Casualties");
      Hand(P2, "Lightning Bolt", "Darksteel Gargoyle", "Beacon of Destruction", "Seal of Fire");

      Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Wurmcoil Engine", "Nantuko Shade");
      Battlefield(P2, "Vivid Creek", "Island", "Steam Vents", "Vivid Crag", "Trip Noose", "Trip Noose", "Island",
        "Mountain", "Mountain", "Wall of Denial");

      RunGame(maxTurnCount: 2);
    }

    public void Num09()
    {
      Hand(P1, "Plains", "Plains", "Deathless Angel", "Baneslayer Angel");
      Hand(P2, "Volcanic Fallout", "Leatherback Baloth");
      Battlefield(P1, "Plains", "Student of Warfare", "Plains", "Angelic Wall", "Plains", "Trip Noose", "Plains");
      Battlefield(P2, "Copperline Gorge", "Llanowar Elves", "Forest", "Sword of Fire and Ice",
        "Sword of Feast and Famine", "Rootbound Crag", "Raging Ravine", "Fires of Yavimaya", "Raging Ravine");

      RunGame(maxTurnCount: 1);
    }

    public void Num10()
    {
      Hand(P1, "Plains", "Plains", "Deathless Angel", "Baneslayer Angel");
      Hand(P2, "Volcanic Fallout");
      Battlefield(P1, "Plains", "Student of Warfare", "Plains", "Angelic Wall", "Plains", "Trip Noose", "Plains",
        "Plains");
      Battlefield(P2, "Copperline Gorge", "Llanowar Elves", "Forest", "Sword of Fire and Ice",
        "Sword of Feast and Famine", "Rootbound Crag", "Raging Ravine", "Fires of Yavimaya", "Raging Ravine",
        "Leatherback Baloth");

      RunGame(maxTurnCount: 1);
    }

    public void Num11()
    {
      Hand(P1, "Plains", "Forest", "Sword of Feast and Famine", "Sword of Body and Mind");
      Hand(P2, "Baneslayer Angel", "Deathless Angel");

      Battlefield(P1, "Razorverge Thicket", "Student of Warfare", "Razorverge Thicket", "Forest",
        C("Troll Ascetic").IsEquipedWith("Sword of Feast and Famine"), "Sunpetal Grove", "Sunpetal Grove");

      Battlefield(P2, "Plains", "Plains", "White Knight", "Plains", "Student of Warfare", "Plains", "Trip Noose",
        "Plains", "Wall of Reverence", "Plains");


      RunGame(maxTurnCount: 1);
    }

    public void Num12()
    {
      Hand(P1, "Pestilence", "Voice of Grace", "Corrupt", "Rune of Protection: Black");
      Hand(P2, "Day of Judgment", "Elesh Norn, Grand Cenobite", "Student of Warfare");
      Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Plains", "Plains", "Opal Acrolith", "Pestilence");
      Battlefield(P2, "Plains", "Plains", "Plains", "Plains", "Wall of Reverence", "Plains", "Wall of Reverence",
        "Hero of Bladehold", "Trip Noose");

      RunGame(maxTurnCount: 2);
    }

    public void Num13()
    {
      Hand(P1, "Cloak of Mists", "Annul");
      Hand(P2, "Elite Archers");
      Battlefield(P1, "Forest", "Pouncing Jaguar", "Island", "Island", "Cave Tiger", "Island", "Fog Bank", "Mobile Fort",
        "Hawkeater Moth", "Elvish Herder", "Barrin's Codex", "Forest", "Karn, Silver Golem", "Dragon Blood");
      Battlefield(P2, "Forest", "Elvish Herder", "Plains", "Plains", "Acridian", "Plains", "Forest", "Cradle Guard",
        "Plains", "Plains", "Hawkeater Moth", "Worship", "Plains", "Blanchwood Treefolk");

      RunGame(maxTurnCount: 2);
    }


    public void Num14()
    {
      Hand(P1, "Fires of Yavimaya");

      Battlefield(P1, "Forest", "Llanowar Elves", "Mountain", "Wall of Blossoms", "Rootbound Crag", "Fires of Yavimaya",
        "Mountain", "Copperline Gorge", "Scoria Wurm", "Mountain", "Wall of Blossoms", "Endless Wurm");

      Battlefield(P2, "Plains", "Plains", "Plains", "Plains", "Glorious Anthem", "Trip Noose", "Plains", "Plains",
        "Deathless Angel", "Trip Noose", "Plains", "Student of Warfare", "Plains");

      RunGame(2);
    }

    public void Num15()
    {
      Hand(P1, "Plains", "Mountain", "Thundering Giant", "Disciple of Grace");
      Hand(P2, "Phyrexian Ghoul", "Mountain", "Swamp", "Swamp", "Destructive Urge", "Phyrexian Ghoul",
        "Hopping Automaton");
      Battlefield(P1, "Mountain", "Mountain", "Plains", "Hopping Automaton");
      Battlefield(P2, "Swamp", "Swamp");

      RunGame(2);
    }

    public void Num16()
    {
      Hand(P1, "Exhume", "Pestilence", "Phyrexian Ghoul", "Blood Vassal", "Swamp");
      Hand(P2, "Power Sink", "Power Sink", "Cloak of Mists", "Waylay", "Waylay", "Sandbar Serpent", "Launch");
      Battlefield(P1, "Swamp", "Swamp", "Swamp", "Bog Raiders", "Phyrexian Ghoul", "Swamp", "Hopping Automaton", "Swamp",
        "Hopping Automaton");
      Battlefield(P2, "Plains", "Plains", "Rune of Protection: Black", "Plains", "Sanctum Custodian");

      RunGame(1);
    }

    public void Num17()
    {
      Hand(P1, "Annul", "Corrupt", "Abyssal Horror", "Pestilence");
      Hand(P2, "Healing Salve", "Grafted Skullcap", "Healing Salve", "Plains");

      Battlefield(P1, "Swamp", "Swamp", "Swamp", C("Mana Leech").IsEnchantedWith("Pacifism"), "Swamp", "Pestilence",
        C("Carrion Beetles").IsEnchantedWith("Diabolic Servitude"), "Blood Vassal");
      Battlefield(P2, "Plains", "Plains", "Disciple of Grace", "Plains", "Plains", "Rune of Protection: Black", "Plains");

      RunGame(1);
    }

    public void Num18()
    {
      Hand(P1, "Pegasus Charger", "Congregate", "Plains");
      Hand(P2, "Congregate", "Pacifism", "Karn, Silver Golem");
      Battlefield(P1, "Plains", "Mountain", "Opal Gargoyle", "Plains", "Dragon Blood", "Mountain", "Opal Acrolith",
        "Dromosaur", "Plains");
      Battlefield(P2, C("Forest").IsEnchantedWith("Fertile Ground"), "Plains", "Plains", "Opal Acrolith", "Plains",
        "Plains");

      RunGame(4);
    }

    public void Num19()
    {
      Hand(P1, "Tolarian Winds", "Windfall", "Imaginary Pet");
      Hand(P2, "Corrupt", "Corrupt", "Corrupt");
      Battlefield(P1, "Forest", "Remote Isle", "Forest", "Island", "Disruptive Student", "Forest", "Karn, Silver Golem",
        "Forest", "Blanchwood Treefolk", "Worn Powerstone", "Wizard Mentor", "Retaliation");
      Battlefield(P2, "Swamp", "Swamp", "Unworthy Dead", "Swamp", "Lurking Evil", "Swamp", "Phyrexian Ghoul",
        "No Rest for the Wicked", "Priest of Gix", "Pestilence", "Swamp");

      RunGame(2);
    }

    public void Num20()
    {
      Hand(P1, "Remote Isle", "Miscalculation");
      Hand(P2, "Plains");
      Battlefield(P1, "Swamp", "Island", "Island", "Spawning Pool", "Swamp", "Fog of Gnats", "Island",
        "Phyrexian Debaser", "Island", "Phyrexian Broodlings", "Swamp", "Phyrexian Broodlings");
      Battlefield(P2, "Plains", "Tragic Poet", "Plains", C("Expendable Troops").IsEnchantedWith("Brilliant Halo"),
        "Disciple of Grace", "Mountain", "Songstitcher", "Mountain", "Pyromancy", "Mountain", "Mountain");

      RunGame(2);
    }

    public void Num21()
    {
      Hand(P1, "Swamp", "Swamp");
      Hand(P2, "Befoul", "Pestilence", "Ravenous Skirge", "Angelic Page");
      Battlefield(P1, "Blasted Landscape", "Island", "Swamp", "Phyrexian Ghoul", "Island", "Phyrexian Debaser", "Swamp",
        "Peregrine Drake", "Pit Trap", "Ravenous Skirge");
      Battlefield(P2, "Plains", "Mother of Runes", "Swamp", "Swamp", "Swamp", "Phyrexian Debaser", "Swamp",
        "Skirge Familiar");

      RunGame(2);
    }
    
    public void Num22()
    {
      Hand(P1, "Forest", "Opal Champion", "Simian Grunts", "Forest");
      Hand(P2, "Beast of Burden", "Island");
      Battlefield(P1, "Forbidding Watchtower", "Forest", "Jhoira's Toolbox", "Plains", "Plains", "Forest",
        "Sustainer of the Realm");
      Battlefield(P2, "Island", "Swamp", C("Walking Sponge").IsEnchantedWith("Cessation"), "Island", "Jhoira's Toolbox",
        "Island", "Phyrexian Debaser", "Endoskeleton", "Swamp", "Phyrexian Reclamation");

      RunGame(2);
    }

    public void Num23()
    {
      Hand(P1, "Swamp", "Plains", "Plains");
      Hand(P2, "Opal Champion", "Thran Weaponry", "Cessation", "Path of Peace", "Forest");
      Battlefield(P1, "Plains", "Swamp", "Swamp", "Phyrexian Broodlings", "Plains", "Phyrexian Debaser", "Swamp",
        "Phyrexian Plaguelord", "Fog of Gnats");
      Battlefield(P2, "Plains", C("Mother of Runes").IsEnchantedWith("Rancor").IsEnchantedWith("Cessation"), "Forest",
        "Forest", "Yavimaya Granger", "Forest", "Ticking Gnomes", "Plains", "Expendable Troops");

      RunGame(2);
    }

    public void Num24()
    {
      Hand(P1, "Mobile Fort", "Angelic Page", "Plains", "Sanctum Custodian");
      Hand(P2, "Karn, Silver Golem", "Symbiosis", "Island");
      Battlefield(P1, "Phyrexian Tower", "Swamp", "Plains", "Phyrexian Ghoul", "Forbidding Watchtower",
        "Hopping Automaton", "Plains", "Karn, Silver Golem");
      Battlefield(P2, "Forest", "Island", "Island",
        C("Walking Sponge").IsEnchantedWith("Cloak of Mists").IsEnchantedWith("Launch"), "Island", "Forest",
        "Hopping Automaton");

      RunGame(2);
    }

    public void Num25()
    {
      Hand(P1, "Plains");
      Hand(P2, "Yavimaya Scion", "Ostracize", "Treefolk Mystic", "Unworthy Dead", "Forest", "Deranged Hermit");
      Battlefield(P1, "Plains", "Mother of Runes", "Plains", "Plains", "Plains", "Angelic Curator", "Expendable Troops",
        "Plains", "Opal Avenger", "Mother of Runes", "Devout Harpist");
      Battlefield(P2, "Swamp", "Swamp", C("Fog of Gnats").IsEnchantedWith("Cessation"), "Forest", "Phyrexian Ghoul",
        "Forest", "Phyrexian Debaser", "Swamp");

      RunGame(2);
    }
  }
}