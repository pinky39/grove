namespace Grove.Ai.TimingRules
{
  using System.Linq;
  using Gameplay.States;

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
      if (p.Controller.IsActive && Turn.Step == Step.BeginningOfCombat)
      {
        return p.Controller.Opponent.Battlefield.CreaturesThatCanBlock.Count() > 0;
      }

      // damage attackers
      if (!p.Controller.IsActive && Turn.Step == Step.DeclareAttackers)
      {
        return Combat.Attackers.Count() > 0;
      }

      // eot or when owner of ability is in trouble
      if ((!p.Controller.IsActive && Turn.Step == Step.EndOfTurn) || Stack.CanBeDestroyedByTopSpell(p.Card))
      {
        return true;
      }

      return false;
    }
  }
}