namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using Core;

  public class WriteCardRatings : Task
  {
    private readonly CardDatabase _cardDatabase;

    public WriteCardRatings(CardDatabase cardDatabase)
    {
      _cardDatabase = cardDatabase;
    }    

    public override void Execute(Arguments arguments)
    {
      var downloader = new RatingDownloader();
      var filename = arguments["filename"];

      var ratedCards = _cardDatabase.GetCardNames()
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
  }
}