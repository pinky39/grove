namespace Grove.Utils
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using Core;

  public class Tasks
  {
    public CardDatabase CardDatabase { get; set; }

    public void WriteCardList(string filename)
    {
      var cardNames = CardDatabase.GetCardNames();

      Console.WriteLine("Writing {0}...", filename);
      using (var writer = new StreamWriter(filename, append: true))
      {
        foreach (var cardName in cardNames)
        {
          writer.WriteLine(cardName);
        }
      }
    }

    public void WriteCardRatings(string filename)
    {
      var downloader = new RatingDownloader();

      var ratedCards = CardDatabase.GetCardNames()
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