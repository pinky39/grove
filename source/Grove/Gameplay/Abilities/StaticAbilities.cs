﻿namespace Grove.Gameplay.Abilities
{
  using Infrastructure;
  using Misc;

  public class StaticAbilities : GameObject, IHashable
  {
    private readonly TrackableList<StaticAbility> _abilities = new TrackableList<StaticAbility>();

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }
    
    public void Add(StaticAbility ability)
    {
      _abilities.Add(ability);
    }

    public void Remove(StaticAbility ability)
    {
      _abilities.Remove(ability);
      ability.Dispose();
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;

      _abilities.Initialize(game.ChangeTracker, card);

      foreach (var ability in _abilities)
      {
        ability.Initialize(card, game);
      }
    }

    public void DisableAll()
    {
      foreach (var ability in _abilities)
      {
        ability.Disable();
      }
    }

    public void EnableAll()
    {
      foreach (var ability in _abilities)
      {
        ability.Enable();
      }
    }
  }
}