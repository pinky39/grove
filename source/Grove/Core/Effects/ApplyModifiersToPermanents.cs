namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;

  public class ApplyModifiersToPermanents : Effect
  {
    private readonly Func<ApplyModifiersToPermanents, Card, bool> _filter;
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();

    private ApplyModifiersToPermanents() {}

    public ApplyModifiersToPermanents(params ModifierFactory[] modifiers) : this(null , modifiers) {}

    public ApplyModifiersToPermanents(Func<Effect, Card, bool> filter, params ModifierFactory[] modifiers)
    {
      _filter = filter ?? delegate { return true; };
      _modifiers.AddRange(modifiers);
    }


    public override int CalculateToughnessReduction(Card card)
    {
      if ((Target == null || card.Controller == Target) && _filter(this, card))
      {
        return ToughnessReduction.GetValue(X);
      }

      return 0;
    }

    protected override void ResolveEffect()
    {
      if (Target != null)
      {
        ApplyModifierToPlayersPermanents(Target.Player());
        return;
      }

      foreach (var player in Players)
      {
        ApplyModifierToPlayersPermanents(player);
      }
    }

    private void ApplyModifierToPlayersPermanents(Player player)
    {
      foreach (var creature in player.Battlefield)
      {
        if (!_filter(this, creature))
          continue;

        foreach (var modifierFactory in _modifiers)
        {
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,
              Target = creature,
              X = X
            };

          var modifier = modifierFactory().Initialize(p, Game);
          creature.AddModifier(modifier);
        }
      }
    }
  }
}