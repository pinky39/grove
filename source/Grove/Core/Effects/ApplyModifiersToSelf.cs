namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class ApplyModifiersToSelf : Effect
  {
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
    private Zone _zoneRestriction;

    private ApplyModifiersToSelf() {}

    public ApplyModifiersToSelf(params CardModifierFactory[] modifiers)
    {
      _modifiers.AddRange(modifiers);
    }

    protected override void Initialize()
    {
      _zoneRestriction = Source.OwningCard.Zone;
    }

    public override bool AffectsEffectSource
    {
      get { return true; }
    }

    public override int CalculateToughnessReduction(Card card)
    {
      return card == Source.OwningCard ? ToughnessReduction.GetValue(X) : 0;
    }

    public override bool CanBeResolved()
    {
      return _zoneRestriction == Source.OwningCard.Zone
        && base.CanBeResolved();
    }

    protected override void ResolveEffect()
    {
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };


      foreach (var modifier in _modifiers.Select(modifierFactory => modifierFactory()))
      {
        Source.OwningCard.AddModifier(modifier, p);
      }
    }
  }
}