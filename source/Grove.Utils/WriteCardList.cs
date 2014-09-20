using System;
using System.Linq;

namespace Grove.Utils
{
  public class WriteCardList : Task
  {
    public override bool Execute(Arguments arguments)
    {
      foreach (var cardName in Cards.All.Select(x => x.Name))
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