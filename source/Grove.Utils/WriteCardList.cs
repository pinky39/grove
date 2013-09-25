namespace Grove.Utils
{
  using System;
  using Gameplay;

  public class WriteCardList : Task
  {
    private readonly CardDatabase _cardDatabase;

    public WriteCardList(CardDatabase cardDatabase, CardFactory cardFactory)
    {
      _cardDatabase = cardDatabase;
      _cardDatabase.Initialize(cardFactory.CreateAll());
    }

    public override bool Execute(Arguments arguments)
    {
      var cardNames = _cardDatabase.GetCardNames();

      foreach (var cardName in cardNames)
      {
        Console.WriteLine(cardName);
      }

      return true;
    }

    public override void Usage()
    {
      Console.WriteLine("usage: ugrove list\n\nWrites available card names to stdout.");
    }
  }
}