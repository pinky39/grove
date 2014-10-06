using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Grove.Utils
{
  public class WriteCardRatings : Task
  {
    public override bool Execute(Arguments arguments)
    {
      var downloader = new RatingDownloader();
      var filename = arguments["f"];

      var ratedCards = Cards.All.Select(x => x.Name)
        .Select(x => new RatedCard {Name = x})
        .ToList();

      if (File.Exists(filename))
      {
        Console.WriteLine("Reading existing ratings from {0}...", filename);
        ReadExistingRatings(filename, ratedCards);
      }

      foreach (var ratedCard in ratedCards)
      {
        string rarity = "";
        ratedCard.Rating = ratedCard.Rating ?? downloader.TryDownloadRating(ratedCard.Name, out rarity) ?? 3.0m;
        ratedCard.Rarity = string.IsNullOrEmpty(ratedCard.Rarity) ? rarity : ratedCard.Rarity;
      }

      using (var writer = new StreamWriter(filename))
      {
        foreach (var ratedCard in ratedCards)
        {
          writer.WriteLine("{0};{1};{2}",
            ratedCard.Name, ratedCard.Rarity,
            ratedCard.Rating.GetValueOrDefault()
              .ToString("f", CultureInfo.InvariantCulture));
        }
      }

      return true;
    }

    private static void ReadExistingRatings(string filename, IEnumerable<RatedCard> ratedCards)
    {
      var ratedCardsDictionary = ratedCards.ToDictionary(x => x.Name);

      using (var reader = new StreamReader(filename))
      {
        var line = reader.ReadLine();

        while (line != null)
        {
          var cardInfo = line.Split(new[] {";"}, 3, StringSplitOptions.RemoveEmptyEntries);

          if (cardInfo.Length == 3)
          {
            if (ratedCardsDictionary.ContainsKey(cardInfo[0]))
            {
              var card = ratedCardsDictionary[cardInfo[0]];
              card.Rarity = cardInfo[1];
              card.Rating = Decimal.Parse(cardInfo[2], CultureInfo.InvariantCulture);              
            }
          }

          line = reader.ReadLine();
        }
      }
    }

    public override void Usage()
    {
      Console.WriteLine(
        "usage: ugrove rate f=ratings.txt\n\nDownloads missing ratings from gatherer and writes them to ratings.txt file.");
    }
  }
}