namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectFlicker : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Card = x,
            Rank = CalculateOpponentCardRank(x, p)
          })
        .Where(x => x.Rank < 0)
        .OrderBy(x => x.Rank)
        .Select(x => x.Card)
        .Take(2)
        .ToList();

      var yours = p.Candidates<Card>(ControlledBy.SpellOwner)        
        .Where(x => x.ConvertedCost > 2)
        .OrderByDescending(x => x.ConvertedCost)
        .Take(2);

      candidates.AddRange(yours);

      return Group(candidates, p.MinTargetCount());
    }

    private int CalculateOpponentCardRank(Card card, TargetingRuleParameters p)
    {
      if (card.Owner == p.Controller)
        return -2;

      if (card.HasAttachments)
        return -1;

      return 0;
    }
  }
}