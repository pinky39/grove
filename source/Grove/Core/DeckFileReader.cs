namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.IO;

  public class DeckFileRow
  {
      public string CardName { get; set; }
      public int Count { get; set; }
  }

  public class DeckFileReader
  {       
    public static List<DeckFileRow> Read(string filename)
    {
      using (var reader = new StreamReader(filename))
      {
        return ReadFile(reader);
      }      
    }
    
    public static List<DeckFileRow> Read(Stream stream)
    {      
      using (var reader = new StreamReader(stream))
      {
        return ReadFile(reader);
      }      
    }

    private static List<DeckFileRow> ReadFile(StreamReader reader) {
      var records = new List<DeckFileRow>();
      string line;
      var lineNumber = 0;

      while ((line = reader.ReadLine()) != null)
      {
        lineNumber++;
        line = line.Trim();

        if (line.Trim().Length == 0)
          continue;
          
        if (line.StartsWith("#"))
          continue;
          
        records.Add(ParseRecord(line, lineNumber));
      }
      return records;
    }

    private static void ThrowParsingError(int lineNumber)
    {
      throw new InvalidOperationException(String.Format("Error parsing line {0}.", lineNumber));
    }

    private static DeckFileRow ParseRecord(string line, int lineNumber)
    {
      var tokens = line.Split(new[]{" "}, 2, StringSplitOptions.RemoveEmptyEntries);

      if (tokens.Length != 2)
        ThrowParsingError(lineNumber);

      int numOfCopies;
      if (!int.TryParse(tokens[0], out numOfCopies))
        ThrowParsingError(lineNumber);

      return new DeckFileRow {CardName = tokens[1], Count = numOfCopies};            
    }
  }
}