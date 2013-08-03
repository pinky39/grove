namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class LooseEvasion : TargetingRule
  {
    private readonly List<Static> _abilities = new List<Static>();

    private LooseEvasion() {}

    public LooseEvasion(params Static[] abilities)
    {
      _abilities.AddRange(abilities);
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(c => c.CanAttack || c.CanBlock())
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
        .ThenByDescending(x => x.Card.Score)
        .Select(x => x.Card);

      return Group(candidates, p.MinTargetCount());
    }
  }
}