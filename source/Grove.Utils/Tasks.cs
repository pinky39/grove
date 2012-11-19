namespace Grove.Utils
{
  using System;
  using System.IO;
  using System.Linq;
  using System.Net;
  using System.Text;
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

    public void FetchCardRatings(string filename)
    {
      var ratedCards = CardDatabase.GetCardNames()
        .Select(x => new RatedCard
          {
            Name = x,
          });

      foreach (var ratedCard in ratedCards)
      {
        ratedCard.Rating = FetchRatingFromGatherer(ratedCard.Name);
      }

      using (var writer = new StreamWriter(filename, append: true))
      {
        foreach (var ratedCard in ratedCards)
        {
          writer.WriteLine("{0} {1}", ratedCard.Rating, ratedCard.Name);
        }
      }
    }

    private decimal FetchRatingFromGatherer(string cardName)
    {
      var multiverseId = FetchMultiverseId(cardName);

      var url = String.Format(@"http://gatherer.wizards.com/Pages/Card/Details.aspx?multiverseid={0}", multiverseId);
      var request = CreateRequest(url);

      using (var response = request.GetResponse())
      using (var reader = new StreamReader(response.GetResponseStream()))
      {
        var content = reader.ReadToEnd();
      }

      return 3; // todo
    }

    private string FetchMultiverseId(string cardName)
    {
      var encodedCardName = EncodeCardName(cardName);
      var searchByNameUrl = String.Format(@"http://gatherer.wizards.com/Pages/Search/Default.aspx?name={0}",
        encodedCardName);
      var request = CreateRequest(searchByNameUrl);

      using (var response = request.GetResponse())
      {
        return ParseMultiverseIdFromUrl(response.Headers["Location"]);
      }
    }

    private HttpWebRequest CreateRequest(string url)
    {
      var request = (HttpWebRequest) WebRequest.Create(url);
      request.UserAgent = "fetch_comunity_ratings";
      request.Method = "GET";
      return request;
    }

    private string ParseMultiverseIdFromUrl(string url)
    {
      // /Pages/Card/Details.aspx?multiverseid=221892
      throw new NotImplementedException();
    }

    private string EncodeCardName(string cardName)
    {
      var parts = cardName.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
      var builder = new StringBuilder();

      foreach (var part in parts)
      {
        builder.Append("+[");
        builder.Append(part);
        builder.Append("]");
      }

      return builder.ToString();
    }
  }
}