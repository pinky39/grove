namespace Grove.AI.TimingRules
{
  using System;

  public class WhenTopSpellIsCounterable : TimingRule
  {
    private readonly Func<TimingRuleParameters, int?> _counterCost;

    private WhenTopSpellIsCounterable()
    {
    }

    public WhenTopSpellIsCounterable(int? counterCost = null)
    {
      _counterCost = delegate { return counterCost; };
    }

    public WhenTopSpellIsCounterable(Func<TimingRuleParameters, int?> counterCost)
    {
      _counterCost = counterCost;
    }

    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
    {
      if (Stack.IsEmpty)
        return false;

      if (Stack.TopSpell.Controller == p.Controller)
        return false;

      var counterCost = _counterCost(p);

      return !counterCost.HasValue ||
        !p.Controller.Opponent.HasMana(counterCost.Value);
    }
  }
}