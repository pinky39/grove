namespace Grove.Gameplay.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Gameplay.Effects;

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