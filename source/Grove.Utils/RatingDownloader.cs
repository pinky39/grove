namespace Grove.Utils
{
  using System;
  using System.Globalization;
  using System.IO;
  using System.Net;
  using System.Text;
  using System.Text.RegularExpressions;

  public class RatingDownloader
  {
    private static readonly Regex RatingPattern = new Regex(@"textRatingValue.*>(\d\.\d*)</span>", RegexOptions.Compiled);
    private static readonly Regex RarityPattern = new Regex(@">(Common|Rare|Uncommon)</span></div>", RegexOptions.Compiled);


    public Decimal? TryDownloadRating(string cardName, out string rarity)
    {
      Console.Write("Downloading rating for: {0}...", cardName);
      var encodedCardName = EncodeCardName(cardName);
      
      var url = String.Format(@"http://gatherer.wizards.com/Pages/Search/Default.aspx?name={0}",
        encodedCardName);
      
      var request = CreateRequest(url);

      using (var response = request.GetResponse())
      using (var reader = new StreamReader(response.GetResponseStream()))
      {
        var content = reader.ReadToEnd();
        var rating = TryParseRating(content);
        
        rarity = TryParseRarity(content);

        if (rating.HasValue)
        {
          Console.WriteLine("Ok");
          return rating.Value;
        }

        Console.WriteLine("Failed");
        return null;
      }      
    }

    private static decimal? TryParseRating(string content)
    {      
      var match = RatingPattern.Match(content);
      if (match.Success)
      {
        decimal rating;
        if (decimal.TryParse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out rating))
        {
          return rating;
        }
      }

      return null;
    }

    private static string TryParseRarity(string content)
    {
      var match = RarityPattern.Match(content);
      if (match.Success)
      {
        return match.Groups[1].Value[0].ToString();
      }

      return null;
    }

    private HttpWebRequest CreateRequest(string url)
    {
      var request = (HttpWebRequest) WebRequest.Create(url);
      request.UserAgent = "fetch_comunity_ratings";
      request.Method = "GET";
      return request;
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