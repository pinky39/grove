namespace Grove.Core.Ai.TimingRules
{
  using Modifiers;

  public class IncreaseOwnersPowerOrToughness : TimingRule
  {
    private readonly Value _power;
    private readonly Value _toughness;

    private IncreaseOwnersPowerOrToughness() {}

    public IncreaseOwnersPowerOrToughness(Value power, Value toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var power = _power.GetValue(p.X);
      var toughness = _toughness.GetValue(p.X);

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