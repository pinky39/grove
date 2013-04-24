namespace Grove.Tournament
{
  using System.Collections.Generic;

  public class CardRatings
  {
    private readonly Dictionary<string, double> _ratings;

    public CardRatings(Dictionary<string, double> ratings)
    {
      _ratings = ratings;
    }

    public double GetRating(string cardname)
    {
      return _ratings[cardname];
    }
  }
}