namespace Grove.Core.Decisions.Machine
{
  using System.Linq;
  using Grove.Core.Targeting;
  using Results;

  public class CardSelector
  {
    public static void ExecuteQueury(SelectCards decision, bool descending)
    {
      ChosenCards cards = decision.Game.GenerateTargets()
        .Where(x => x.IsCard())
        .Select(x => x.Card())
        .Where<Card>(x => x.Controller == decision.Controller)
        .Where(decision.Validator)        
        .OrderByDescending<Card, int>(x => descending ? x.Score : -x.Score)        
        .Take<Card>(decision.MaxCount)
        .ToList<Card>();

      decision.Result = cards;
    }
  }
}