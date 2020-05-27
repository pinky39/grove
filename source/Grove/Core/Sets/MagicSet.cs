namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using AI;
  using Infrastructure;

  public class MagicSet
  {
    private readonly Dictionary<string, int> _boosterPack = new Dictionary<string, int>();
    private readonly List<string> _commons = new List<string>();
    private readonly List<string> _rares = new List<string>();
    private readonly List<string> _mythics = new List<string>();
    private readonly Dictionary<string, double> _ratings = new Dictionary<string, double>();
    private readonly Dictionary<string, int> _tournamentPack = new Dictionary<string, int>();
    private readonly List<string> _uncommons = new List<string>();

    public MagicSet(string name, string content)
    {
      Name = name;
      Load(content.Split(new[] {'\n'}));
    }

    public CardRatings Ratings
    {
      get { return new CardRatings(_ratings); }
    }

    public string Name { get; private set; }

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
        case (Rarity.M):
          _mythics.Add(name);
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

    private void Load(string[] rows)
    {
      var sections = new Dictionary<string, Action<string>>
        {
          {"!Cards", ParseCard},
          {"!Tournament Pack", ParseTournamentPack},
          {"!Booster Pack", ParseBoosterPack}
        };

      Action<string> currentSection = null;

      int lineNum = 0;
      foreach (var row in rows)
      {
        lineNum++;
        var trimmed = row.Trim();
        try
        {          
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
        catch (Exception ex)
        {
          var message = String.Format(
            "Could not load set '{0}' due to error on line {1}:\n\n{2}.",
            Name, lineNum, trimmed);
          
          throw new InvalidOperationException(message, ex);
        }
      }
    }

    private CardInfo GetRandomCommon()
    {
      return new CardInfo(_commons[RandomEx.Next(0, _commons.Count)], Rarity.C, Name);
    }

    private CardInfo GetRandomUncommon()
    {
      return new CardInfo(_uncommons[RandomEx.Next(0, _uncommons.Count)], Rarity.U, Name);
    }

    private CardInfo GetRandomRare()
    {
      return new CardInfo(_rares[RandomEx.Next(0, _rares.Count)], Rarity.R, Name);
    }

    private CardInfo GetRandomMythic()
    {
      return new CardInfo(_mythics[RandomEx.Next(0, _mythics.Count)], Rarity.M, Name);
    }

    public List<CardInfo> GenerateMixedPack(int boosterCount, int tournamentCount)
    {
      var pack = new List<CardInfo>();

      for (var i = 0; i < boosterCount; i++)
      {
        pack.AddRange(GenerateBoosterPack());
      }

      for (var i = 0; i < tournamentCount; i++)
      {
        pack.AddRange(GenerateTournamentPack());
      }

      return pack;
    }

    public List<CardInfo> GenerateTournamentPack()
    {
      return GeneratePack(_tournamentPack);
    }

    private List<CardInfo> GeneratePack(Dictionary<string, int> settings)
    {
      var cards = new Dictionary<string, CardInfo>();
      bool hasMythics = _mythics.Count > 0;

      for (var i = 0; i < settings["Rares"]; i++)
      {
        CardInfo random;
        do
        {
          var isMythic = RandomEx.Next(8) == 0;
          
          if (hasMythics && isMythic)
          {
            random = GetRandomMythic();
          }
          else
          {
            random = GetRandomRare();
          }
          
        } while (cards.ContainsKey(random.Name));

        cards.Add(random.Name, random);
      }

      for (var i = 0; i < settings["Uncommons"]; i++)
      {
        CardInfo random;
        do
        {
          random = GetRandomUncommon();
        } while (cards.ContainsKey(random.Name));

        cards.Add(random.Name, random);
      }

      for (var i = 0; i < settings["Commons"]; i++)
      {
        CardInfo random;
        do
        {
          random = GetRandomCommon();
        } while (cards.ContainsKey(random.Name));

        cards.Add(random.Name, random);
      }

      return cards.Values.ToList();
    }

    public List<CardInfo> GenerateBoosterPack()
    {
      return GeneratePack(_boosterPack);
    }

    public static int GetLimitedCode(string tournamentPack, string[] boosterPacks)
    {
      var code = tournamentPack.GetHashCode();

      foreach (var set in boosterPacks)
      {
        code += set.GetHashCode();
      }

      return code;
    }
  }
}