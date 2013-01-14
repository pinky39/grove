namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Grove.Core.Targeting;
  using Modifiers;

  public class ApplyModifiersToTargets : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();
    public Value ToughnessReduction = 0;

    public override int CalculateToughnessReduction(Card card)
    {
      foreach (var target in ValidTargets)
      {
        if (target == card)
        {
          return ToughnessReduction.GetValue(X);
        }
      }

      return 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidTargets)
      {
        foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, target, X, Game))
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