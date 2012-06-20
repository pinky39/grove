namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static class PlayersEx
  {
    public static IEnumerable<Card> Permanents(this Players players)
    {
      return players.SelectMany(x => x.Battlefield);
    }

    public static void DestroyPermanents(this Players players, Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      foreach (var permanent in players.Permanents().ToList())
      {
        if (filter(permanent))
        {
          permanent.Destroy();
        }
      } 
    }
  }
}