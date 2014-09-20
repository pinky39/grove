namespace Grove.AI
{
  using System.Collections.Generic;

  public class CardRatings
  {
    private readonly Dictionary<string, double> _ratings;

    public CardRatings(Dictionary<string, double> ratings)
    {
      _ratings = new Dictionary<string, double>(ratings);
    }

    public double GetRating(string cardname)
    {
      return _ratings[cardname];
    }

    public static CardRatings Merge(CardRatings ratings1, CardRatings ratings2)
    {
      var result = new CardRatings(ratings1._ratings);

      foreach (var pair in ratings2._ratings)
      {
        result._ratings[pair.Key] = pair.Value;
      }

      return result;
    }
  }
}