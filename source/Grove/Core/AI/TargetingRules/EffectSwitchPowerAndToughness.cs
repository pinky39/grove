namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectSwitchPowerAndToughness : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = new List<Card>();
      
      if (p.Controller.IsActive && Turn.Step == Step.DeclareBlockers)
      {
        candidates.AddRange(
          p.Candidates<Card>(ControlledBy.SpellOwner)
            .Where(x => x.IsAttacker && !x.HasBlockers)
            .Where(x => x.Toughness > x.Power)
            .OrderByDescending(x => x.Toughness));                              
      }

      if ((!p.Controller.IsActive && Turn.Step == Step.EndOfTurn) || (p.Controller.IsActive && Turn.Step == Step.BeginningOfCombat))
      {
        candidates.AddRange(
          p.Candidates<Card>(ControlledBy.Opponent)
            .Where(x => x.Damage >= x.Power)
            .OrderByDescending(x => x.Score)        
          );
      }

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}