namespace Grove.Artifical.CombatRules
{
  using System;
  using Gameplay.ManaHandling;

  public class Regenerate : CombatRule
  {
    private readonly IManaAmount _cost;

    private Regenerate() {}

    public Regenerate(IManaAmount cost)
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