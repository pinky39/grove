namespace Grove.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;

  public class ExileAllCards : Effect
  {
    private readonly Func<Effect, Card, bool> _filter;
    private readonly Zone _from;

    private ExileAllCards() {}

    public ExileAllCards(Zone from = Zone.Battlefield, Func<Effect, Card, bool> filter = null)
    {
      _from = from;
      _filter = filter ?? delegate { return true; };
      SetTags(EffectTag.Exile);
    }

    protected override void ResolveEffect()
    {
      var permanents = GetCards();

      foreach (var permanent in permanents)
      {
        permanent.ExileFrom(_from);
      }
    }

    private IEnumerable<Card> GetCards()
    {
      var forTarget = Target != null && Target.IsPlayer();

      if (_from == Zone.Battlefield)
      {
        if (forTarget)
        {
          return Target.Player().Battlefield.Where(c => _filter(this, c)).ToList();
        }

        return Players.Permanents().Where(c => _filter(this, c)).ToList();
      }

      if (_from == Zone.Graveyard)
      {
        if (forTarget)
        {
          return Target.Player().Graveyard.Where(c => _filter(this, c)).ToList();
        }

        return Players.Player1.Graveyard.Where(c => _filter(this, c)).Concat(Players.Player2.Graveyard.Where(c => _filter(this, c))).ToList();
      }

      throw new NotSupportedException("Zone is not supported: " + _from);
    }
  }
}