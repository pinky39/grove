namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;
  using Player;

  public class ApplyModifiersToPlayer : Effect
  {
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();
    private readonly Func<Effect, Player> _selector;

    private ApplyModifiersToPlayer() {}

    public ApplyModifiersToPlayer(Func<Effect, Player> selector, params ModifierFactory[] modifiers)
    {
      _selector = selector;
      _modifiers.AddRange(modifiers);
    }

    protected override void ResolveEffect()
    {
      foreach (var modifier in CreateModifiers())
      {
        _selector(this).AddModifier(modifier);
      }
    }

    private IEnumerable<Modifier> CreateModifiers()
    {
      foreach (var modifierFactory in _modifiers)
      {
        var p = new ModifierParameters
          {
            SourceEffect = this,
            SourceCard = Source.OwningCard,
            Target = _selector(this),
            X = X
          };

        yield return modifierFactory().Initialize(p, Game);
      }
    }
  }
}