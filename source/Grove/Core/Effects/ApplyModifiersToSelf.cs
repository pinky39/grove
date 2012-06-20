namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Zones;

  public class ApplyModifiersToSelf : Effect
  {
    private readonly List<IModifier> _selfModifiers = new List<IModifier>();

    public void Modifiers(params IModifierFactory[] modifiersFactories)
    {
      var modifiers = modifiersFactories.CreateModifiers(
        source: Source.OwningCard,
        target: Source.OwningCard,
        x: X);

      _selfModifiers.AddRange(modifiers);
    }

    protected override void ResolveEffect()
    {
      if (Source.OwningCard.Zone == Zone.Battlefield)
      {
        foreach (var modifier in _selfModifiers)
        {
          Source.OwningCard.AddModifier(modifier);
        }
      }
    }
  }
}