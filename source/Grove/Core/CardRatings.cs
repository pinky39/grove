namespace Grove.Core
{
  using System;
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
      throw new NotImplementedException();
    }
  }
}