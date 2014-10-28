namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class ApplyModifiersToCard : Effect
  {
    private readonly DynParam<Card> _card;
    private Zone _zoneRestriction;
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();

    private ApplyModifiersToCard() {}

    public ApplyModifiersToCard(DynParam<Card> card, params CardModifierFactory[] modifiers)
    {
      _card = card;
      _modifiers.AddRange(modifiers);

      RegisterDynamicParameters(card);
    }

    protected override void Initialize()
    {
      _zoneRestriction = _card.Value.Zone;
    }

    public override int CalculateToughnessReduction(Card card)
    {
      return card == _card.Value ? ToughnessReduction.GetValue(X) : 0;
    }

    public override bool CanBeResolved()
    {
      return _zoneRestriction == _card.Value.Zone
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
        _card.Value.AddModifier(modifier, p);
      }
    }
  }
}