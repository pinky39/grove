namespace Grove.AI.TimingRules
{
  using System;

  public class WhenTopSpellIs : TimingRule
  {
    private readonly Func<Card, bool> _selector;
    private readonly ControlledBy _controlledBy;

    private WhenTopSpellIs() {}

    public WhenTopSpellIs(Func<Card, bool> selector, ControlledBy controlledBy = ControlledBy.Any)
    {
      _selector = selector;
      _controlledBy = controlledBy;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      if (Stack.IsEmpty)
        return false;

      var spellController = Stack.TopSpell.Controller;

      if (_controlledBy == ControlledBy.Any ||
        spellController == p.Controller && _controlledBy == ControlledBy.SpellOwner ||
        spellController != p.Controller && _controlledBy == ControlledBy.Opponent)
      {
        return _selector(Stack.TopSpell.Source.OwningCard);
      }

      return false;
    }
  }
}