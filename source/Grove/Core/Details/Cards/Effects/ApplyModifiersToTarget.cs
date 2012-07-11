namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;

  public class ApplyModifiersToTarget : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    protected override void ResolveEffect()
    {
      foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, Target().Card(), X))
      {
        Target().Card().AddModifier(modifier);
      }
    }

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }
  }
}