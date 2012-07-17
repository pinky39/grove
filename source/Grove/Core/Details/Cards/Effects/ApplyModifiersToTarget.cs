namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;

  public class ApplyModifiersToTarget : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

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