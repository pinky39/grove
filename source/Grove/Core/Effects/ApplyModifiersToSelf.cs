namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Zones;

  public class ApplyModifiersToSelf : Effect
  {
    private readonly List<IModifierFactory> _selfModifiers = new List<IModifierFactory>();

    public void Modifiers(params IModifierFactory[] modifiersFactories)
    {
      _selfModifiers.AddRange(modifiersFactories);
    }

    private IEnumerable<Modifier> CreateSelfModifiers()
    {
      return _selfModifiers.CreateModifiers(Source.OwningCard, Source.OwningCard, X);
    }

    protected override void ResolveEffect()
    {
      if (Source.OwningCard.Zone == Zone.Battlefield)
      {
        foreach (var modifier in CreateSelfModifiers())
        {
          Source.OwningCard.AddModifier(modifier);
        }
      }
    }
  }
}