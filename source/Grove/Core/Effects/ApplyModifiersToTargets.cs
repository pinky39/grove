namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class ApplyModifiersToTargets : Effect
  {
    private readonly List<CardModifierFactory> _cardModifiers = new List<CardModifierFactory>();
    private readonly List<PlayerModifierFactory> _playerModifiers = new List<PlayerModifierFactory>();

    private ApplyModifiersToTargets() {}

    public ApplyModifiersToTargets(params CardModifierFactory[] cardModifiers)
    {
      _cardModifiers.AddRange(cardModifiers);
    }

    public ApplyModifiersToTargets(params PlayerModifierFactory[] playerModifiers)
    {
      _playerModifiers.AddRange(playerModifiers);
    }

    public override int CalculateToughnessReduction(Card card)
    {
      foreach (var target in ValidEffectTargets)
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
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      foreach (var target in ValidEffectTargets)
      {
        if (target.IsCard())
        {
          foreach (var modifier in _cardModifiers.Select(factory => factory()))
          {
            target.Card().AddModifier(modifier, p);
          }
        }

        else if (target.IsPlayer())
        {
          foreach (var modifier in _playerModifiers.Select(factory => factory()))
          {
            target.Player().AddModifier(modifier, p);
          }
        }
      }
    }
  }
}