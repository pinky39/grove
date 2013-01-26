namespace Grove.Core.Ai.TimingRules
{
  using Modifiers;

  public class BoostOwningCardPowerAndToughness : TimingRule
  {
    public Value Power;
    public Value Toughness;

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var power = Power.GetValue(p.X);
      var toughness = Toughness.GetValue(p.X);

      if (toughness > 0 && Stack.CanBeDealtLeathalDamageByTopSpell(p.Card))
      {
        return true;
      }

      if (Turn.Step == Step.DeclareBlockers && p.Controller.IsActive && p.Card.IsAttacker)
      {
        return QuickCombat.CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(
          attacker: p.Card,
          blockers: Combat.GetBlockers(p.Card),
          powerIncrease: power,
          toughnessIncrease: toughness) > 0;
      }

      if (Turn.Step == Step.DeclareBlockers && !p.Controller.IsActive && p.Card.IsBlocker)
      {
        return QuickCombat.CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(
          blocker: p.Card,
          attacker: Combat.GetAttacker(p.Card),
          powerIncrease: power,
          toughnessIncrease: toughness) > 0;
      }

      return false;
    }
  }
}