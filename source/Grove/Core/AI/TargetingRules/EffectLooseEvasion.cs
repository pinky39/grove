namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectLooseEvasion : TargetingRule
  {
    private readonly List<Static> _abilities = new List<Static>();

    private EffectLooseEvasion() {}

    public EffectLooseEvasion(params Static[] abilities)
    {
      _abilities.AddRange(abilities);
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(c => c.IsAttacker || c.CanBlock())
        .Select(c =>
          {
            for (var i = 0; i < _abilities.Count; i++)
            {
              var ability = _abilities[i];

              if (c.Has().Has(ability))
                return new
                  {
                    Card = c,
                    Rank = i
                  };
            }

            return new
              {
                Card = c,
                Rank = -1
              };
          })
        .Where(x => x.Rank != -1)
        .OrderBy(x => x.Rank)
        .ThenByDescending(x => x.Card.Power*2 + x.Card.Toughness)
        .Select(x => x.Card);

      return Group(candidates, p.MinTargetCount());
    }
  }
}