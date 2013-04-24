namespace Grove.Tournament
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;

  public class MagicSet
  {
    private static readonly Random Random = new Random();
    private readonly Dictionary<string, int> _boosterPack = new Dictionary<string, int>();
    private readonly List<string> _commons = new List<string>();
    private readonly List<string> _rares = new List<string>();
    private readonly Dictionary<string, double> _ratings = new Dictionary<string, double>();
    private readonly Dictionary<string, int> _tournamentPack = new Dictionary<string, int>();
    private readonly List<string> _uncommons = new List<string>();

    public MagicSet(string filename)
    {
      LoadFromFile(filename);
    }

    public CardRatings Ratings {get {return new CardRatings(_ratings);}}

    private void ParseCard(string line)
    {
      var tokens = line.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);

      var name = tokens[0].Trim();
      var rarity = (Rarity) Enum.Parse(typeof (Rarity), tokens[1].Trim());
      var rating = double.Parse(tokens[2].Trim(), CultureInfo.InvariantCulture);

      _ratings[name] = rating;

      switch (rarity)
      {
        case (Rarity.C):
          _commons.Add(name);
          break;
        case (Rarity.U):
          _uncommons.Add(name);
          break;
        case (Rarity.R):
          _rares.Add(name);
          break;
      }
    }

    private void ParseTournamentPack(string line)
    {
      ParseProperty(line, _tournamentPack);
    }

    private void ParseBoosterPack(string line)
    {
      ParseProperty(line, _boosterPack);
    }

    private static void ParseProperty(string line, Dictionary<string, int> storage)
    {
      var tokens = line.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);
      storage[tokens[0].Trim()] = int.Parse(tokens[1].Trim());
    }

    private void LoadFromFile(string filename)
    {
      var rows = File.ReadAllLines(filename);

      var sections = new Dictionary<string, Action<string>>
        {
          {"!Cards", ParseCard},
          {"!Tournament Pack", ParseTournamentPack},
          {"!Booster Pack", ParseBoosterPack}
        };

      Action<string> currentSection = null;

      foreach (var row in rows)
      {
        var trimmed = row.Trim();

        if (trimmed.StartsWith("#"))
          continue;

        if (String.IsNullOrEmpty(trimmed))
          continue;

        if (sections.ContainsKey(trimmed))
        {
          currentSection = sections[trimmed];
          continue;
        }

        if (currentSection == null)
          continue;

        currentSection(trimmed);
      }
    }

    private string GetRandomCommon()
    {
      return _commons[Random.Next(0, _commons.Count)];
    }

    private string GetRandomUncommon()
    {
      return _uncommons[Random.Next(0, _uncommons.Count)];
    }

    private string GetRandomRare()
    {
      return _rares[Random.Next(0, _rares.Count)];
    }

    public List<string> GenerateMixedPack(int boosterCount, int tournamentCount)
    {
      var pack = new List<string>();

      for (int i = 0; i < boosterCount; i++)
      {
        pack.AddRange(GenerateBoosterPack());
      }

      for (int i = 0; i < tournamentCount; i++)
      {
        pack.AddRange(GenerateTournamentPack());
      }

      return pack;
    }
    
    public List<string> GenerateTournamentPack()
    {
      return GeneratePack(_tournamentPack);
    }

    private List<string> GeneratePack(Dictionary<string, int> settings)
    {
      var cards = new List<string>();

      for (var i = 0; i < settings["Rares"]; i++)
      {
        cards.Add(GetRandomRare());
      }

      for (var i = 0; i < settings["Uncommons"]; i++)
      {
        cards.Add(GetRandomUncommon());
      }

      for (var i = 0; i < settings["Commons"]; i++)
      {
        cards.Add(GetRandomCommon());
      }

      return cards;
    }

    public List<string> GenerateBoosterPack()
    {
      return GeneratePack(_boosterPack);
    }
  }
}