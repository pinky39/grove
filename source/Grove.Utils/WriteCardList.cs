namespace Grove.Utils
{
  using System;
  using System.IO;
  using Core;

  public class WriteCardList : Task
  {
    private readonly CardDatabase _cardDatabase;

    public WriteCardList(CardDatabase cardDatabase)
    {
      _cardDatabase = cardDatabase;
    }
        
    
    public override void Execute(Arguments arguments)
    {
      var filename = arguments["filename"];
      var cardNames = _cardDatabase.GetCardNames();

      Console.WriteLine("Writing {0}...", filename);
      using (var writer = new StreamWriter(filename, append: true))
      {
        foreach (var cardName in cardNames)
        {
          writer.WriteLine(cardName);
        }
      }
    }
  }
}