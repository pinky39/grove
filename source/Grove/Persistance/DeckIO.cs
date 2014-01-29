namespace Grove.Persistance
{
  using System;
  using System.IO;
  using System.Text.RegularExpressions;
  using Gameplay;
  using Gameplay.Sets;
  using Infrastructure;

  public class DeckFile
  {
    private static readonly Regex DescriptionRegex = new Regex(@"#.*Description\:\s*(.+)", RegexOptions.Compiled);
    private static readonly Regex RatingRegex = new Regex(@"#.*Rating\:\s*(.+)", RegexOptions.Compiled);
    private static readonly Regex LimitedCodeRegex = new Regex(@"#.*Limited\:\s*(.+)", RegexOptions.Compiled);

    private static readonly Regex DeckRowRegex = new Regex(@"([0-9]+)\s+([^()]+)\s*(?:\((.)\,\s+(.+)\))*",
      RegexOptions.Compiled);

    public static Deck Read(string name, byte[] content)
    {
      using (var reader = new StreamReader(new MemoryStream(content)))
      {
        string line;
        var lineNumber = 0;

        var deck = new Deck();
        deck.Name = name;

        while ((line = reader.ReadLine()) != null)
        {
          lineNumber++;
          line = line.Trim();

          if (line.Trim().Length == 0)
            continue;

          if (line.StartsWith("#"))
          {
            var match = DescriptionRegex.Match(line);
            if (match.Success)
            {
              deck.Description = match.Groups[1].Value;
              continue;
            }

            match = RatingRegex.Match(line);
            int rating;
            if (match.Success)
            {
              int.TryParse(match.Groups[1].Value, out rating);
              deck.Rating = rating;
              continue;
            }

            match = LimitedCodeRegex.Match(line);
            int limitedCode;
            if (match.Success)
            {
              int.TryParse(match.Groups[1].Value, out limitedCode);
              deck.LimitedCode = limitedCode;
              continue;
            }

            continue;
          }

          var row = ParseRow(line, lineNumber);

          deck.AddCard(row.Card, row.Count);
        }
        return deck;
      }
    }

    public static Deck Read(string filename)
    {
      return Read(Path.GetFileNameWithoutExtension(filename), File.ReadAllBytes(filename));
    }


    public static byte[] Write(Deck deck)
    {
      using (var stream = new MemoryStream())
      {
        using (var writer = new StreamWriter(stream))
        {
          if (!String.IsNullOrEmpty(deck.Description))
            writer.WriteLine("# Description: {0}", deck.Description);

          if (deck.Rating.HasValue)
            writer.WriteLine("# Rating: {0}", deck.Rating);

          if (deck.LimitedCode.HasValue)
            writer.WriteLine("# Limited: {0}", deck.LimitedCode);

          foreach (var row in DeckRow.Group(deck))
          {
            if (string.IsNullOrEmpty(row.Card.Set))
            {
              writer.WriteLine("{0} {1}", row.Count, row.Card.Name);
            }
            else
            {
              writer.WriteLine("{0} {1} ({2}, {3})", row.Count, row.Card.Name, row.Card.Rarity, row.Card.Set);
            }
          }
        }
        return stream.ToArray();
      }
    }

    private static DeckRow ParseRow(string line, int lineNumber)
    {
      line = line.Trim();
      var match = DeckRowRegex.Match(line);

      AssertEx.True(match.Success,
        String.Format("Error parsing line {0}.", lineNumber));

      var cardCount = int.Parse(match.Groups[1].Value);
      var cardName = match.Groups[2].Value.Trim();

      if (!match.Groups[3].Success)
      {
        return new DeckRow {Card = new CardInfo(cardName), Count = cardCount};
      }

      var rarity = (Rarity) Enum.Parse(typeof (Rarity), match.Groups[3].Value);
      var set = match.Groups[4].Value;

      return new DeckRow {Card = new CardInfo(cardName, rarity, set), Count = cardCount};
    }
  }
}