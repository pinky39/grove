namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class ApplyModifiersToTargets : Effect
  {
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();

    private ApplyModifiersToTargets() {}

    public ApplyModifiersToTargets(params CardModifierFactory[] modifiers)
    {
      _modifiers.AddRange(modifiers);
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

      foreach (Card target in ValidEffectTargets)
      {
        foreach (var modifier in _modifiers.Select(factory => factory()))
        {
          target.AddModifier(modifier, p);
        }
      }
    }
  }
}