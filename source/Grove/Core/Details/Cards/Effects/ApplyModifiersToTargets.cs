namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;

  public class ApplyModifiersToTargets : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();
    public Value ToughnessReduction = 0;

    public override bool NeedsTargets { get { return true; } }

    public override int CalculateToughnessReduction(Card creature)
    {
      foreach (var target in Targets)
      {
        if (target == creature)
        {
          return ToughnessReduction.GetValue(X);
        }
      }

      return 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in Targets)
      {
        foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, target, X))
        {
          target.AddModifier(modifier);
        }
      }
    }

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }
  }
}