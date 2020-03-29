namespace Grove.Tests
{
  using System;
  using System.Linq;
  using Grove.AI;
  using Grove.Tests.Infrastructure;
  using Grove.Media;
  using Xunit;
  using Grove;
  using System.Collections.Generic;

  public class DraftFacts : Scenario
  {
    public DraftFacts()
    {
      MediaLibrary.LoadSets();
    }

    //[Fact]
    public void BuildDeck1()
    {
      var library = new List<string> {
        "Void Snare",
"Void Snare",
"Void Snare",
"Chief Engineer",
"Welkin Tern",
"Mind Sculpt",
"Military Intelligence",
"Coral Barrier",
"Dissipate",
"Frost Lynx",
"Illusory Angel",
"Wall of Frost",
"Frost Lynx",
"Cancel",
"Coral Barrier",
"Encrust",
"Amphin Pathmage",
"Glacial Crasher",
"Chronostutter",
"Dauntless River Marshal",
"Kinsbaile Skirmisher",
"Kinsbaile Skirmisher",
"Kinsbaile Skirmisher",
"Raise the Alarm",
"Raise the Alarm",
"Ephemeral Shields",
"Solemn Offering",
"Geist of the Moors",
"Heliod's Pilgrim",
"Midnight Guard",
"Divine Verdict",
"Marked by Honor",
"Tireless Missionaries",
"Sanctified Charge",
"Boonweaver Giant",
"Boonweaver Giant",
"Foundry Street Denizen",
"Crowd's Favor",
"Crowd's Favor",
"Hammerhand",
"Torch Fiend",
"Soul of Shandalar",
"Reclamation Sage",
"Shaman of Spring",
"Typhoid Rats"
      };


      var deck = DeckBuilder.BuildDeck(
         library.Select(x => new CardInfo(x)),
         MediaLibrary.GetSet("M15").Ratings, decks => decks.First());
    }

    //[Fact]
    public void DraftLibraries()
    {
      Deck DeckEvaluator(List<Deck> candidates)
      {
        return candidates.First();
      }

      var sets = new[] { "M15", "M15", "M15" };
      var ratings = MediaLibrary.GetSet(sets[0]).Ratings;

      var runner = new DraftRunner();

      var results = runner.Run(
        playerCount: 8,
        sets: sets,
        ratings: ratings);

      Equal(8, results.Libraries.Count);
      True(results.Libraries.All(x => x.Count == 45));

      PrintLibraries(results.Libraries);

      var decks = new List<Deck>();
      foreach (var library in results.Libraries)
      {
        var deck = DeckBuilder.BuildDeck(library, ratings, DeckEvaluator);
        decks.Add(deck);
      }
                  
      PrintDecks(decks);     
    }

    private void PrintDecks(List<Deck> decks)
    {
      var i = 0;
      foreach (var deck in decks)
      {
        PrintDeckOrLibrary($"Deck {i++}", deck);
      }
    }

    private static void PrintLibraries(List<List<CardInfo>> libraries)
    {
      var i = 0;
      foreach (var library in libraries)
      {
        PrintDeckOrLibrary($"Library {i++}", library);
      }
    }

    private static void PrintDeckOrLibrary(string title, IEnumerable<CardInfo> cards)
    {
      var grouped = cards.Select(x => Grove.Cards.All[x.Name])
          .GroupBy(c =>
          {
            if (c.IsColorless())
              return "colorless";

            return String.Join(" ", c.Colors.Select(x => x.ToString()));
          })
          .Select(x => new { Color = x.Key, Cards = x.ToList() })
          .OrderByDescending(x => x.Cards.Count)
          .Select(x =>
          {
            return new
            {
              Color = x.Color,
              CreatureCount = x.Cards.Count(c => c.Type.Creature),
              SpellCount = x.Cards.Count(c => !c.Type.Creature),
              Cards = x.Cards.Select(y => new
              {
                Color = x.Color,
                Name = y.Name,
                Type = y.Type,
                Cost = y.ConvertedCost
              }).OrderBy(y => y.Cost).ToArray()
            };
          }).ToArray();

      Console.WriteLine($"\n\n{title}\n\n");
      foreach (var group in grouped)
      {
        Console.WriteLine($"\n{group.Color} -- Creatures: '{group.CreatureCount}' " +
          $"Spells: {group.SpellCount}\n");

        foreach (var card in group.Cards)
        {
          Console.WriteLine($"'{card.Name}' '{card.Type}' {card.Cost} {card.Color}");
        }
      }
    }
  }
}