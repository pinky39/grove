namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
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
        return new Deck(DeckFileReader.Read(stream)
          .SelectMany(x => CreateCard(x.CardName, x.Count, controller)));                
      }
    }
    
    private IEnumerable<Card> CreateCard(string name, int numOfCopies, Player controller)
    {
      for (var i = 0; i < numOfCopies; i++)
      {
        yield return _cardDatabase.CreateCard(name, controller);
      }
    }    
  }
}