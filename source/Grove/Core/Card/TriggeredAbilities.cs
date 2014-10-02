namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;
  using Modifiers;

  public class TriggeredAbilities : GameObject, IAcceptsCardModifier, IHashable
  {
    private readonly TrackableList<TriggeredAbility> _abilities;

    private TriggeredAbilities() { }

    public TriggeredAbilities(IEnumerable<TriggeredAbility> triggeredAbilities) { _abilities = new TrackableList<TriggeredAbility>(triggeredAbilities); }
    public void Accept(ICardModifier modifier) { modifier.Apply(this); }
    public int CalculateHash(HashCalculator calc) { return calc.Calculate(_abilities); }
    public void Add(TriggeredAbility ability) { _abilities.Add(ability); }

    public void Remove(TriggeredAbility ability)
    {
      _abilities.Remove(ability);
      ability.Dispose();
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;

      _abilities.Initialize(game.ChangeTracker, card);

      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.Initialize(card, game);
      }
    }

    public IEnumerable<TriggeredAbility> GetFiltered(Func<TriggeredAbility, bool> filter)
    {
      return _abilities.Where(filter);
    }

    public void DisableAll()
    {
      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.IsEnabled = false;
      }
    }

    public void EnableAll()
    {
      foreach (var triggeredAbility in _abilities)
      {
        triggeredAbility.IsEnabled = true;
      }
    }
  }
}