namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;

  public class ApplyModifiersToTarget : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();
    public Value ToughnessReduction = 0;

    public override int CalculateToughnessReduction(Card creature)
    {
      return Target() == creature ? ToughnessReduction.GetValue(X) : 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, Target(), X))
      {
        Target().AddModifier(modifier);
      }
    }

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }
  }
}