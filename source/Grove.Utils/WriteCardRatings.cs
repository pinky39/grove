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
        ratedCard.Rating = ratedCard.Rating ?? downloader.TryDownloadRating(ratedCard.Name) ?? 3.0m;
      }

      using (var writer = new StreamWriter(filename))
      {
        foreach (var ratedCard in ratedCards)
        {
          writer.WriteLine("{0};{1}",
            ratedCard.Rating.GetValueOrDefault()
              .ToString("f", CultureInfo.InvariantCulture), ratedCard.Name);
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
          var ratingAndName = line.Split(new[] {";"}, 2, StringSplitOptions.RemoveEmptyEntries);

          if (ratingAndName.Length == 2)
          {
            if (ratedCardsDictionary.ContainsKey(ratingAndName[1]))
            {
              ratedCardsDictionary[ratingAndName[1]].Rating = Decimal.Parse(ratingAndName[0],
                CultureInfo.InvariantCulture);
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