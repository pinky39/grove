namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Modifiers;

  public class ApplyModifiersToPlayer : Effect
  {
    private readonly List<PlayerModifierFactory> _modifiers = new List<PlayerModifierFactory>();
    private readonly Func<Effect, Player> _selector;

    private ApplyModifiersToPlayer() {}

    public ApplyModifiersToPlayer(Func<Effect, Player> selector, params PlayerModifierFactory[] modifiers)
    {
      _selector = selector;
      _modifiers.AddRange(modifiers);
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
        _selector(this).AddModifier(modifier, p);
      }
    }
  }
}