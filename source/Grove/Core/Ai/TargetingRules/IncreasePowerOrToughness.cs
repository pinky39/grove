namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class IncreasePowerOrToughness : TargetingRule
  {
    public int? Power;
    public int? Toughness;
    public bool UntilEot = true;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var power = Power ?? p.MaxX;
      var toughness = Toughness ?? p.MaxX;

      var candidates = None<Card>();

      if (p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates = GetCandidatesForAttackerPowerToughnessIncrease(power, toughness, p);
      }

      else if (!p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates = GetCandidatesForBlockerPowerToughnessIncrease(power, toughness, p);
      }

      else if (UntilEot && toughness > 0)
      {
        candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
          .Where(x => Stack.CanBeDealtLeathalDamageByTopSpell(Target.Card(x)));
      }

      else if (!UntilEot &&
        (!p.Controller.IsActive && Turn.Step == Step.EndOfTurn ||
          p.Card.IsPermanent && Stack.CanBeDestroyedByTopSpell(p.Card)))
      {
        candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
          .OrderByDescending(x => x.Score);
      }

      return Group(candidates, p.MinTargetCount());
    }
  }
}