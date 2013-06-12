namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;
  using Zones;

  public class ApplyModifiersToSelf : Effect
  {
    private readonly List<CardModifierFactory> _selfModifiers = new List<CardModifierFactory>();

    private ApplyModifiersToSelf() {}

    public ApplyModifiersToSelf(params CardModifierFactory[] modifiers)
    {
      _selfModifiers.AddRange(modifiers);
    }

    public override bool TargetsEffectSource { get { return true; } }

    public override int CalculateToughnessReduction(Card card)
    {
      return card == Source.OwningCard ? ToughnessReduction.GetValue(X) : 0;
    }

    protected override void ResolveEffect()
    {
      var target = Source.OwningCard;

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      if (Source.OwningCard.Zone == Zone.Battlefield)
      {
        foreach (var modifier in _selfModifiers.Select(modifierFactory => modifierFactory()))
        {
          target.AddModifier(modifier, p);
        }
      }
    }
  }
}