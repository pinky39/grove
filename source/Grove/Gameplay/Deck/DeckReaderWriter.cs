namespace Grove.Gameplay.Deck
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text.RegularExpressions;
  using Card.Factory;

  public class DeckReaderWriter
  {
    private static readonly Regex DescriptionRegex = new Regex(@"#.*Description\:\s*(.+)", RegexOptions.Compiled);
    private static readonly Regex RatingRegex = new Regex(@"#.*Rating\:\s*(.+)", RegexOptions.Compiled);

    public Deck Read(string filename, CardDatabase cardDatabase)
    {
      using (var reader = new StreamReader(filename))
      {
        return ReadFile(Path.GetFileNameWithoutExtension(filename), reader, cardDatabase);
      }
    }

    public void Write(Deck deck, string filename)
    {
      using (var writer = new StreamWriter(filename))
      {
        writer.WriteLine("# Description: {0}", deck.Description);
        writer.WriteLine("# Rating: {0}", deck.Rating);
        writer.WriteLine();

        foreach (DeckRow row in deck.AsRows())
        {
          writer.WriteLine("{0} {1}", row.Count, row.CardName);
        }
      }
    }

    private static Deck ReadFile(string name, StreamReader reader, CardDatabase cardDatabase)
    {
      var records = new List<DeckRow>();
      string line;
      int lineNumber = 0;
      string description = String.Empty;
      int rating = 3;

      while ((line = reader.ReadLine()) != null)
      {
        lineNumber++;
        line = line.Trim();

        if (line.Trim().Length == 0)
          continue;

        if (line.StartsWith("#"))
        {
          System.Text.RegularExpressions.Match match = DescriptionRegex.Match(line);
          if (match.Success)
          {
            description = match.Groups[1].Value;
            continue;
          }

          match = RatingRegex.Match(line);
          if (match.Success)
          {
            int.TryParse(match.Groups[1].Value, out rating);
          }
          continue;
        }


        records.Add(ParseRecord(line, lineNumber));
      }

      return new Deck(records.SelectMany(x => x), cardDatabase, name, rating, description);
    }

    private static void ThrowParsingError(int lineNumber)
    {
      throw new InvalidOperationException(String.Format("Error parsing line {0}.", lineNumber));
    }

    private static DeckRow ParseRecord(string line, int lineNumber)
    {
      string[] tokens = line.Split(new[] {" "}, 2, StringSplitOptions.RemoveEmptyEntries);

      if (tokens.Length != 2)
        ThrowParsingError(lineNumber);

      int numOfCopies;
      if (!int.TryParse(tokens[0], out numOfCopies))
        ThrowParsingError(lineNumber);

      return new DeckRow {CardName = tokens[1], Count = numOfCopies};
    }
  }
}