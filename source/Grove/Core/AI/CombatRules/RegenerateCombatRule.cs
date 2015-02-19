namespace Grove.AI.CombatRules
{
  public class RegenerateCombatRule : CombatRule
  {
    private readonly ManaAmount _cost;

    private RegenerateCombatRule() {}

    public RegenerateCombatRule(ManaAmount cost)
    {
      _cost = cost;
    }

    public override void Apply(CombatAbilities combatAbilities)
    {
      if (OwningCard.Controller.HasMana(_cost, ManaUsage.Abilities))
        combatAbilities.CanRegenerate = true;
    }
  }
}