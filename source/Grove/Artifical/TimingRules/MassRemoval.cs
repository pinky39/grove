namespace Grove.Artifical.TimingRules
{
  using System;
  using System.Linq;
  using Gameplay.States;

  [Serializable]
  public class MassRemoval : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (
        Stack.TopSpell != null &&
          Stack.TopSpell.Controller == p.Controller.Opponent &&
            Stack.TopSpell.HasCategory(EffectCategories.Protector | EffectCategories.ToughnessIncrease))
      {
        return true;
      }

      // remove potential blockers
      if (p.Controller.IsActive && Turn.Step == Step.BeginningOfCombat && Stack.IsEmpty)
      {
        return p.Controller.Opponent.Battlefield.CreaturesThatCanBlock.Count() > 0;
      }

      // damage attackers
      if (!p.Controller.IsActive && Turn.Step == Step.DeclareAttackers && Stack.IsEmpty)
      {
        return Combat.Attackers.Count() > 0;
      }

      // eot or when owner of ability is in trouble
      if ((!p.Controller.IsActive && Turn.Step == Step.EndOfTurn && Stack.IsEmpty) || Stack.CanBeDestroyedByTopSpell(p.Card))
      {
        return true;
      }

      return false;
    }
  }
}