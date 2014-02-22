namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using AI;
  using Grove.Infrastructure;

  public class MagicSet
  {
    private readonly Dictionary<string, int> _boosterPack = new Dictionary<string, int>();
    private readonly List<string> _commons = new List<string>();
    private readonly List<string> _rares = new List<string>();
    private readonly Dictionary<string, double> _ratings = new Dictionary<string, double>();
    private readonly Dictionary<string, int> _tournamentPack = new Dictionary<string, int>();
    private readonly List<string> _uncommons = new List<string>();

    public MagicSet(string name, string content)
    {
      Name = name;
      Load(content.Split(new[]{'\n'}));
    }

    public CardRatings Ratings { get { return new CardRatings(_ratings); } }
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

      for (var i = 0; i < settings["Rares"]; i++)
      {
        CardInfo random;
        do
        {
          random = GetRandomRare();
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