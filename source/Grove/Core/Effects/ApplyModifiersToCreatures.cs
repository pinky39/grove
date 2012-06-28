namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using Modifiers;

  public class ApplyModifiersToCreatures : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public Func<IEffectSource, Card, bool> Filter = delegate { return true; };

    public void Modifiers(params IModifierFactory[] modifiers)
    {
      _modifierFactories.AddRange(modifiers);
    }

    protected override void ResolveEffect()
    {            
      if (Targets.Any())
      {
        ApplyModifierToPlayersCreatures(Target.Player());
        return;
      }

      foreach (var player in Players)
      {
        ApplyModifierToPlayersCreatures(player);
      }
    }

    private void ApplyModifierToPlayersCreatures(Player player)
    {
      foreach (var creature in player.Battlefield.Creatures)
      {
        if (!Filter(Source, creature))
          continue;
        
        foreach (var modifier in _modifierFactories.CreateModifiers(Source.OwningCard, creature, X))
        {
          creature.AddModifier(modifier);
        }
      }
    }
  }
}