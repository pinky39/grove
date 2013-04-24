namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using Card;
  using Modifiers;
  using Targeting;

  public class ApplyModifiersToTargets : Effect
  {
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();

    private ApplyModifiersToTargets() {}

    public ApplyModifiersToTargets(params ModifierFactory[] modifiers)
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
      foreach (var target in ValidEffectTargets)
      {
        foreach (var modifierFactory in _modifiers)
        {
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,
              Target = target,
              X = X
            };

          var modifier = modifierFactory().Initialize(p, Game);
          target.AddModifier(modifier);
        }
      }
    }
  }
}