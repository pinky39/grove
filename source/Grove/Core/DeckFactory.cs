namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Ui;

  public class DeckFactory
  {
    private readonly CardDatabase _cardDatabase;

    public DeckFactory(CardDatabase cardDatabase)
    {
      _cardDatabase = cardDatabase;
    }

    public Deck CreateDeck(string name, Player controller)
    {
      if (name == "dummy")
      {
        return new Deck(CreateCard("Uncastable", 60, controller));
      }

      using (var stream = MediaLibrary.GetDeck(name))
      {
        return ParseDeckFile(stream, controller);
      }
    }

    private static IEnumerable<Card> ParseComment(string line)
    {
      return line.StartsWith("#") ? new Card[]{} : null;
    }

    private static IEnumerable<Card> SkipEmptyLines(string line)
    {
      return line.Trim().Length == 0 ? new Card[]{} : null;
    }

    private static void ThrowParsingError(int lineNumber)
    {
      throw new InvalidOperationException(String.Format("Error parsing line {0}.", lineNumber));
    }

    private IEnumerable<Card> CreateCard(string name, int numOfCopies, Player controller)
    {
      for (var i = 0; i < numOfCopies; i++)
      {
        yield return _cardDatabase.CreateCard(name, controller);
      }
    }

    private IEnumerable<Card> ParseCard(string line, int lineNumber, Player controller)
    {
      var tokens = line.Split(new[]{" "}, 2, StringSplitOptions.RemoveEmptyEntries);

      if (tokens.Length != 2)
        ThrowParsingError(lineNumber);

      int numOfCopies;
      if (!int.TryParse(tokens[0], out numOfCopies))
        ThrowParsingError(lineNumber);

      return CreateCard(tokens[1], numOfCopies, controller);
    }

    private Deck ParseDeckFile(Stream stream, Player controller)
    {
      var cards = new List<Card>();

      using (var reader = new StreamReader(stream))
      {
        string line;
        var lineNumber = 0;

        while ((line = reader.ReadLine()) != null)
        {
          lineNumber++;
          line = line.Trim();

          var copies =
            SkipEmptyLines(line) ??
              ParseComment(line) ??
                ParseCard(line, lineNumber, controller);

          cards.AddRange(copies);
        }
      }
      return new Deck(cards.ToArray());
    }
  }
}