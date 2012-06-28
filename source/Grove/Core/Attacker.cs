namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Controllers;
  using Controllers.Results;
  using Infrastructure;
  using Messages;

  [Copyable]
  public class Attacker : IHashable
  {
    private readonly TrackableList<Damage> _assignedDamage;
    private readonly TrackableList<Blocker> _blockers;
    private readonly Card _card;
    private readonly Trackable<bool> _isBlocked;
    private readonly Players _players;
    private readonly Publisher _publisher;

    private Attacker(Card card, ChangeTracker changeTracker, Players players, Publisher publisher)
    {
      _card = card;
      _players = players;
      _publisher = publisher;
      _blockers = new TrackableList<Blocker>(changeTracker);
      _assignedDamage = new TrackableList<Damage>(changeTracker);
      _isBlocked = new Trackable<bool>(changeTracker);
    }

    private Attacker() {}

    public IEnumerable<Blocker> Blockers { get { return _blockers; } }
    public int BlockersCount { get { return _blockers.Count; } }
    public IEnumerable<Blocker> BlockersInDamageAssignmentOrder { get { return _blockers.OrderBy(x => x.DamageAssignmentOrder); } }
    public Card Card { get { return _card; } }
    public Player Controller { get { return _card.Controller; } }
    public bool HasDeathTouch { get { return _card.Has().Deathtouch; } }
    public bool HasTrample { get { return _card.Has().Trample; } }
    public int LifepointsLeft { get { return _card.LifepointsLeft; } }
    public int TotalDamageThisCanDeal { get { return _card.Power.Value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_isBlocked),
        calc.Calculate(_card),
        calc.Calculate(_blockers),
        calc.Calculate(_assignedDamage));
    }

    public void AddBlocker(Blocker blocker)
    {
      _blockers.Add(blocker);
      _isBlocked.Value = true;
    }

    public void AssignDamage(Card damageSource, int amount)
    {
      var damage = new Damage(damageSource, amount);
      _assignedDamage.Add(damage);
    }

    public bool CanBeBlockedBy(Card creature)
    {
      return _card.CanBeBlockedBy(creature);
    }

    public void DealAssignedDamage()
    {
      foreach (var damage in _assignedDamage)
      {
        _card.DealDamage(damage.Source, damage.Amount, isCombat: true);
      }

      _assignedDamage.Clear();
    }

    public void DistributeDamageToBlockers(Decisions decisions)
    {
      decisions.EnqueueAssignCombatDamage(_card.Controller, this);
    }

    public void DistributeDamageToBlockers(DamageDistribution distribution)
    {
      foreach (var blocker in _blockers)
      {
        blocker.AssignDamage(distribution[blocker]);
      }

      var defender = _players.GetOpponent(_card.Controller);

      if (HasTrample || _isBlocked == false)
      {
        var unassignedDamage = TotalDamageThisCanDeal - distribution.Total;
        defender.AssignDamage(_card, unassignedDamage);
      }
    }

    public bool HasBlocker(Blocker blocker)
    {
      return _blockers.Contains(blocker);
    }

    public void RemoveBlocker(Blocker blocker)
    {
      _blockers.Remove(blocker);
    }

    public void RemoveFromCombat()
    {
      _publisher.Publish(new RemovedFromCombat {Card = Card});

      foreach (var blocker in _blockers)
      {
        blocker.RemoveAttacker();
      }
    }

    public void SetDamageAssignmentOrder(DamageAssignmentOrder damageAssignmentOrder)
    {
      foreach (var blocker in _blockers)
      {
        blocker.DamageAssignmentOrder = damageAssignmentOrder[blocker];
      }
    }

    public static implicit operator Card(Attacker attacker)
    {
      return attacker._card;
    }

    public bool WillBeDealtLeathalCombatDamage()
    {
      return Combat.CanAttackerBeDealtLeathalCombatDamage(Card, _blockers.Select(x => x.Card));
    }

    [Copyable]
    public class Factory : IAttackerFactory
    {
      private readonly ChangeTracker _changeTracker;
      private readonly Players _players;
      private readonly Publisher _publisher;

      public Factory(ChangeTracker changeTracker, Players players, Publisher publisher)
      {
        _changeTracker = changeTracker;
        _players = players;
        _publisher = publisher;
      }

      private Factory() {}

      public Attacker Create(Card card)
      {
        return new Attacker(card, _changeTracker, _players, _publisher);
      }
    }

    public int CalculateGainIfGivenABoost(int power, int toughness)
    {                                    
      if (_blockers.None())
      {
        return 1;
      }

      var blockers = _blockers.Select(x => x.Card);      
      var withoutBoost = Combat.CanAttackerBeDealtLeathalCombatDamage(Card, blockers);

      if (!withoutBoost)
        return 0;
      
      var withBoost = Combat.CanAttackerBeDealtLeathalCombatDamage(Card, blockers, power, toughness);
      return !withBoost ? Card.Score : 0;
    }
  }
}