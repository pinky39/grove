namespace Grove.Core
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Text.RegularExpressions;

  public class DeckFile : IEnumerable<DeckRow>
  {
    private static readonly Regex DescriptionRegex = new Regex(@"#.*Description\:\s*(.+)", RegexOptions.Compiled);
    private static readonly Regex RatingRegex = new Regex(@"#.*Rating\:\s*(.+)", RegexOptions.Compiled);
    private readonly List<DeckRow> _rows = new List<DeckRow>();

    public DeckFile(IEnumerable<DeckRow> rows, string description, int rating)
    {
      Description = description;
      Rating = rating;

      _rows.AddRange(rows);
    }

    public DeckFile()
    {
      Description = String.Empty;
      Rating = 3;
    }

    public string Description { get; set; }
    public int Rating { get; set; }

    public IEnumerable<string> AllCards
    {
      get
      {
        foreach (var row in _rows)
        {
          for (int i = 0; i < row.Count; i++)
          {
            yield return row.CardName;
          }
        }
      }
    }

    public int RowCount  { get { return _rows.Count; }}
      
    public IEnumerator<DeckRow> GetEnumerator()
    {
      return _rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public static DeckFile Read(string filename)
    {
      using (var reader = new StreamReader(filename))
      {
        return ReadFile(reader);
      }
    }

    public static DeckFile Read(Stream stream)
    {
      using (var reader = new StreamReader(stream))
      {
        return ReadFile(reader);
      }
    }
    
    public void Write(string filename)
    {
      using (var writer = new StreamWriter(filename))
      {
        
        writer.WriteLine("# Description: {0}", Description);
        writer.WriteLine("# Rating: {0}", Rating);
        writer.WriteLine();

        foreach (var row in _rows)
        {
          writer.WriteLine("{0} {1}", row.Count, row.CardName);
        }
      }
    }

    private static DeckFile ReadFile(StreamReader reader)
    {
      var records = new List<DeckRow>();
      string line;
      var lineNumber = 0;
      var description = String.Empty;
      var rating = 3;

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
      return new DeckFile(records, description, rating);
    }

    private static void ThrowParsingError(int lineNumber)
    {
      throw new InvalidOperationException(String.Format("Error parsing line {0}.", lineNumber));
    }

    private static DeckRow ParseRecord(string line, int lineNumber)
    {
      var tokens = line.Split(new[] {" "}, 2, StringSplitOptions.RemoveEmptyEntries);

      if (tokens.Length != 2)
        ThrowParsingError(lineNumber);

      int numOfCopies;
      if (!int.TryParse(tokens[0], out numOfCopies))
        ThrowParsingError(lineNumber);

      return new DeckRow {CardName = tokens[1], Count = numOfCopies};
    }

    public void AddRow(DeckRow row)
    {
      _rows.Add(row);
    }

    public void RemoveRow(DeckRow row)
    {
      _rows.Remove(row);
    }
  }
}