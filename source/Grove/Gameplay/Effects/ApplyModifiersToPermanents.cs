namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;

  public class ApplyModifiersToPermanents : Effect
  {
    private readonly ControlledBy _controlledBy;
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
    private readonly Func<ApplyModifiersToPermanents, Card, bool> _selector;

    private ApplyModifiersToPermanents() {}

    public ApplyModifiersToPermanents(params CardModifierFactory[] modifiers) : this(null, modifiers: modifiers) {}

    public ApplyModifiersToPermanents(Func<Effect, Card, bool> selector,
      ControlledBy controlledBy = ControlledBy.Any, params CardModifierFactory[] modifiers)
    {
      _controlledBy = controlledBy;
      _selector = selector ?? delegate { return true; };
      _modifiers.AddRange(modifiers);
    }


    public override int CalculateToughnessReduction(Card card)
    {
      if ((Target == null || card.Controller == Target) && _selector(this, card))
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
        if (!_selector(this, permanent))
          continue;

        foreach (var modifierFactory in _modifiers)
        {
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,              
              X = X
            };

          var modifier = modifierFactory();          
          permanent.AddModifier(modifier, p);
        }
      }
    }
  }
}