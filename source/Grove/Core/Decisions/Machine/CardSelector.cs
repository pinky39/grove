namespace Grove.Core.Decisions.Machine
{
  using System.Linq;
  using Results;
  using Targeting;

  public class CardSelector
  {
    public static void ExecuteQueury(SelectCards decision, bool descending)
    {
      ChosenCards cards =
        decision.Game.GenerateTargets((zone, owner) => owner == decision.Controller && decision.Zone(zone))
          .Where(x => x.IsCard())
          .Select(x => x.Card())
          .Where(decision.Validator)
          .OrderByDescending(x => descending ? x.Score : -x.Score)
          .Take(decision.MaxCount)
          .ToList();

      decision.Result = cards;
    }
  }
}