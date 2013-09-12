namespace Grove.Artifical.TimingRules
{
  public class WhenTopSpellIsCounterable : TimingRule
  {
    private int? _counterCost;

    private WhenTopSpellIsCounterable() {}

    public WhenTopSpellIsCounterable(int? counterCost = null)
    {
      _counterCost = counterCost;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      if (Stack.IsEmpty)
        return false;

      if (Stack.TopSpell.Controller == p.Controller)
        return false;

      return !_counterCost.HasValue ||
        !p.Controller.Opponent.HasMana(_counterCost.Value);
    }
  }
}