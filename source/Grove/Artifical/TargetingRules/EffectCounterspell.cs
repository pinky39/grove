namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class EffectCounterspell : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {      
      var candidates = p.Candidates<Effect>(ControlledBy.Opponent);

      if (!candidates.Contains(Stack.TopSpell))
      {
        return None<Targets>();
      }

      return new[] {new Targets().AddEffect(Stack.TopSpell)};
    }
  }
}