namespace Grove.Persistance
{
  using System;
  using System.IO;
  using System.Text.RegularExpressions;
  using Gameplay;

  public class DeckFile
  {
    private static readonly Regex DescriptionRegex = new Regex(@"#.*Description\:\s*(.+)", RegexOptions.Compiled);
    private static readonly Regex RatingRegex = new Regex(@"#.*Rating\:\s*(.+)", RegexOptions.Compiled);
    private static readonly Regex LimitedCodeRegex = new Regex(@"#.*Limited\:\s*(.+)", RegexOptions.Compiled);


    public static Deck Read(string filename)
    {
      using (var reader = new StreamReader(filename))
      {
        string line;
        var lineNumber = 0;

        var deck = new Deck();
        deck.Name = Path.GetFileNameWithoutExtension(filename);

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

          deck.AddCard(row.CardName, row.Count);
        }
        return deck;
      }
    }


    public static void Write(Deck deck, string filename)
    {
      using (var writer = new StreamWriter(filename))
      {
        if (!String.IsNullOrEmpty(deck.Description))
          writer.WriteLine("# Description: {0}", deck.Description);

        if (deck.Rating.HasValue)
          writer.WriteLine("# Rating: {0}", deck.Rating);

        if (deck.LimitedCode.HasValue)
          writer.WriteLine("# Limited: {0}", deck.LimitedCode);

        foreach (var row in DeckRow.Group(deck))
        {
          writer.WriteLine("{0} {1}", row.Count, row.CardName);
        }
      }
    }

    private static void ThrowParsingError(int lineNumber)
    {
      throw new InvalidOperationException(String.Format("Error parsing line {0}.", lineNumber));
    }

    private static DeckRow ParseRow(string line, int lineNumber)
    {
      var tokens = line.Split(new[] {" "}, 2, StringSplitOptions.RemoveEmptyEntries);

      if (tokens.Length != 2)
        ThrowParsingError(lineNumber);

      int numOfCopies;
      if (!int.TryParse(tokens[0], out numOfCopies))
        ThrowParsingError(lineNumber);

      return new DeckRow {CardName = tokens[1], Count = numOfCopies};
    }
  }
}