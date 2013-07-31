namespace Grove.Artifical.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Decisions.Results;

  public static class QuickDecisions
  {
    public static Ordering OrderTopCards(List<Card> candidates, Player controller)
    {
      var landsInPlay = controller.Battlefield.Lands.Count();

      var needsLands = controller.Hand.Lands.Count() == 0 &&
        landsInPlay <= 5;

      var indices = Enumerable.Repeat(0, candidates.Count)
        .ToArray();

      var ordering = candidates.Select((x, i) =>
        {
          int score;

          if (x.Is().Land)
          {
            score = needsLands ? 100 : 0;
          }
          else if (x.ConvertedCost <= landsInPlay)
          {
            score = x.ConvertedCost;
          }
          else
          {
            score = -x.ConvertedCost;
          }

          return new
            {
              Card = x,
              Index = i,
              Score = score
            };
        })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Index)
        .ToList();

      for (var i = 0; i < ordering.Count; i++)
      {
        indices[ordering[i]] = i;
      }

      return new Ordering(indices);
    }
  }
}