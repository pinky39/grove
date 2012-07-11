namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Zones;

  public class ApplyModifiersToSelf : Effect
  {
    private readonly List<IModifierFactory> _selfModifiers = new List<IModifierFactory>();

    public override bool AffectsSource { get { return true; } }

    public void Modifiers(params IModifierFactory[] modifiersFactories)
    {
      _selfModifiers.AddRange(modifiersFactories);
    }

    private IEnumerable<Modifier> CreateSelfModifiers()
    {
      var target = Source.OwningCard;

      return _selfModifiers.CreateModifiers(Source.OwningCard, target, X);
    }

    protected override void ResolveEffect()
    {
      var target = Source.OwningCard;

      if (Source.OwningCard.Zone == Zone.Battlefield)
      {
        foreach (var modifier in CreateSelfModifiers())
        {
          target.AddModifier(modifier);
        }
      }
    }
  }
}