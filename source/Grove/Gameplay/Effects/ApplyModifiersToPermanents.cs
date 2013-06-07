namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using Misc;
  using Modifiers;
  using Targeting;

  public class ApplyModifiersToPermanents : Effect
  {
    private readonly ControlledBy _controlledBy;
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();
    private readonly Func<ApplyModifiersToPermanents, Card, bool> _permanentFilter;

    private ApplyModifiersToPermanents() {}

    public ApplyModifiersToPermanents(params ModifierFactory[] modifiers) : this(null, modifiers: modifiers) {}

    public ApplyModifiersToPermanents(Func<Effect, Card, bool> permanentFilter,
      ControlledBy controlledBy = ControlledBy.Any, params ModifierFactory[] modifiers)
    {
      _controlledBy = controlledBy;
      _permanentFilter = permanentFilter ?? delegate { return true; };
      _modifiers.AddRange(modifiers);
    }


    public override int CalculateToughnessReduction(Card card)
    {
      if ((Target == null || card.Controller == Target) && _permanentFilter(this, card))
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

      if (_controlledBy == ControlledBy.SpellOwner)
      {
        ApplyModifierToPlayersPermanents(Controller);
        return;
      }

      if (_controlledBy == ControlledBy.Opponent)
      {
        ApplyModifierToPlayersPermanents(Controller.Opponent);
        return;
      }

      foreach (var player in Players)
      {
        ApplyModifierToPlayersPermanents(player);
      }
    }

    private void ApplyModifierToPlayersPermanents(Player player)
    {
      foreach (var permanent in player.Battlefield)
      {
        if (!_permanentFilter(this, permanent))
          continue;

        foreach (var modifierFactory in _modifiers)
        {
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,
              Target = permanent,
              X = X
            };

          var modifier = modifierFactory();
          modifier.Initialize(p, Game);
          permanent.AddModifier(modifier);
        }
      }
    }
  }
}