namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using Grove.Core.Targeting;
  using Modifiers;

  public class ApplyModifiersToPermanents : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public Func<ApplyModifiersToPermanents, Card, bool> Filter = delegate { return true; };
    public Value ToughnessReduction = 0;

    public override int CalculateToughnessReduction(Card card)
    {
      if ((Target() == null || card.Controller == Target()) && Filter(this, card))
      {
        return ToughnessReduction.GetValue(X);
      }

      return 0;
    }

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }

    protected override void ResolveEffect()
    {
      if (Target() != null)
      {
        ApplyModifierToPlayersPermanents(Target().Player());
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
        if (!Filter(this, creature))
          continue;

        foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, creature, X, Game))
        {
          creature.AddModifier(modifier);
        }
      }
    }
  }
}