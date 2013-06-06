namespace Grove.Utils
{
  using System;
  using System.IO;
  using Gameplay;

  public class WriteCardList : Task
  {
    private readonly CardsDictionary _cardsDictionary;

    public WriteCardList(CardsDictionary cardsDictionary)
    {
      _cardsDictionary = cardsDictionary;
    }


    public override void Execute(Arguments arguments)
    {
      var filename = arguments["filename"];
      var cardNames = _cardsDictionary.GetCardNames();

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