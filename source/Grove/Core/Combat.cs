﻿namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Controllers;
  using Details.Combat;
  using Infrastructure;
  using Messages;

  [Copyable]
  public class Combat : IHashable
  {
    private readonly IAttackerFactory _attackerFactory;
    private readonly TrackableList<Attacker> _attackers;
    private readonly IBlockerFactory _blockerFactory;
    private readonly TrackableList<Blocker> _blockers;
    private readonly Players _players;
    private readonly Publisher _publisher;

    private Combat() {}

    public Combat(ChangeTracker changeTracker, IAttackerFactory attackerFactory, IBlockerFactory blockerFactory,
      Publisher publisher, Players players)
    {
      _attackerFactory = attackerFactory;
      _blockerFactory = blockerFactory;
      _publisher = publisher;
      _players = players;
      _attackers = new TrackableList<Attacker>(changeTracker);
      _blockers = new TrackableList<Blocker>(changeTracker);
    }

    public IEnumerable<Attacker> Attackers { get { return _attackers; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_attackers),
        calc.Calculate(_blockers));
    }

    public void AssignCombatDamage(Decisions decisions, bool firstStrike = false)
    {
      var blockers = firstStrike
        ? _blockers.Where(x => x.Card.HasFirstStrike)
        : _blockers.Where(x => x.Card.HasNormalStrike);

      var attackers = firstStrike
        ? _attackers.Where(x => x.Card.HasFirstStrike)
        : _attackers.Where(x => x.Card.HasNormalStrike);

      foreach (var blocker in blockers)
      {
        blocker.DistributeDamageToAttacker();
      }

      foreach (var attacker in attackers)
      {
        attacker.DistributeDamageToBlockers(decisions);
      }
    }

    public bool CanAnyAttackerBeBlockedByAny(IEnumerable<Card> creatures)
    {
      foreach (var attacker in _attackers)
      {
        foreach (var creature in creatures)
        {
          if (attacker.CanBeBlockedBy(creature))
            return true;
        }
      }

      return false;
    }

    public void DealAssignedDamage()
    {
      foreach (var attacker in _attackers)
      {
        attacker.DealAssignedDamage();
      }

      foreach (var blocker in _blockers)
      {
        blocker.DealAssignedDamage();
      }

      _players.Defending.DealAssignedDamage();
    }

    public void JoinAttack(Card card, bool wasDeclared = false)
    {
      var attacker = _attackerFactory.Create(card);
      _attackers.Add(attacker);

      if (!card.Has().Vigilance)
        card.Tap();

      PublishMessage(new AttackerJoinedCombat
        {
          Attacker = attacker,
          WasDeclared = wasDeclared
        });
    }

    public void DeclareAttacker(Card card)
    {
      if (!card.CanAttackThisTurn)
        return;

      JoinAttack(card, wasDeclared: true);
    }

    public void DeclareBlocker(Card cardBlocker, Card cardAttacker)
    {
      var attacker = FindAttacker(cardAttacker);
      var blocker = _blockerFactory.Create(cardBlocker, attacker);

      attacker.AddBlocker(blocker);
      _blockers.Add(blocker);

      _publisher.Publish(new BlockerJoinedCombat
        {
          Blocker = blocker,
          Attacker = attacker
        });
    }

    public bool IsAttacker(Card card)
    {
      return FindAttacker(card) != null;
    }

    public bool IsBlocker(Card card)
    {
      return FindBlocker(card) != null;
    }

    public void Remove(Card card)
    {
      var attacker = FindAttacker(card);

      if (attacker != null)
      {
        _attackers.Remove(attacker);
        attacker.RemoveFromCombat();
        return;
      }

      var blocker = FindBlocker(card);

      if (blocker != null)
      {
        _blockers.Remove(blocker);
        blocker.RemoveFromCombat();
      }
    }

    public void RemoveAll()
    {
      foreach (var blocker in _blockers)
      {
        blocker.RemoveFromCombat();
      }

      foreach (var attacker in _attackers)
      {
        attacker.RemoveFromCombat();
      }

      _blockers.Clear();
      _attackers.Clear();
    }

    public void SetDamageAssignmentOrder(Decisions decisions)
    {
      foreach (var attacker in _attackers)
      {
        var attackerCopy = attacker;
        
        decisions.Enqueue<SetDamageAssignmentOrder>(
          controller: attacker.Controller,
          init: p => p.Attacker = attackerCopy);
      }
    }

    private Attacker FindAttacker(Card cardAttacker)
    {
      return _attackers.FirstOrDefault(a => a.Card == cardAttacker);
    }

    private Blocker FindBlocker(Card cardBlocker)
    {
      return _blockers.FirstOrDefault(b => b.Card == cardBlocker);
    }

    private void PublishMessage<TMessage>(TMessage message)
    {
      _publisher.Publish(message);
    }

    public bool AnyCreaturesWithFirstStrike()
    {
      return _attackers.Any(x => x.Card.HasFirstStrike) ||
        _blockers.Any(x => x.Card.HasFirstStrike);
    }

    public bool AnyCreaturesWithNormalStrike()
    {
      return _attackers.Any(x => x.Card.HasNormalStrike) ||
        _blockers.Any(x => x.Card.HasNormalStrike);
    }

    public bool CanBeDealtLeathalCombatDamage(Card card)
    {
      var attacker = FindAttacker(card);

      if (attacker != null)
      {
        return attacker.CanBeDealtLeathalCombatDamage();
      }

      var blocker = FindBlocker(card);

      if (blocker != null)
      {
        return blocker.WillBeDealtLeathalCombatDamage();
      }

      return false;
    }

    public bool HasBlockers(Card card)
    {
      var attacker = FindAttacker(card);
      return attacker.BlockersCount > 0;
    }

    public bool CanBlockAtLeastOneAttacker(Card card)
    {
      return Attackers.Any(attacker => attacker.CanBeBlockedBy(card));
    }

    public int CountHowManyThisCouldBlock(Card card)
    {
      var opponent = _players.GetOpponent(card.Controller);
      return opponent.Battlefield.CreaturesThatCanAttack.Count(x => x.CanBeBlockedBy(card));
    }

    public bool CouldBeBlockedByAny(Card card)
    {
      var opponent = _players.GetOpponent(card.Controller);
      return opponent.Battlefield.CreaturesThatCanBlock.Any(card.CanBeBlockedBy);
    }

    public bool WillAnyAttackerDealDamageToDefender()
    {
      return GetAttackerWhichWillDealGreatestDamageToDefender() != null;
    }

    public int CalculateDamageAttackerWillDealToPlayer(Card attacker)
    {
      return FindAttacker(attacker).GetDamageThisWillDealToPlayer();
    }

    public Card GetAttackerWhichWillDealGreatestDamageToDefender(Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      return Attackers
        .Where(x => filter(x))
        .OrderByDescending(x => x.GetDamageThisWillDealToPlayer())
        .FirstOrDefault();
    }

    public IEnumerable<Card> GetBlockers(Card attacker)
    {
      return FindAttacker(attacker).Blockers.Select(x => x.Card);
    }

    public Card GetAttacker(Card blocker)
    {
      var attacker = FindBlocker(blocker).Attacker;
      return attacker != null ? attacker.Card : null;
    }

    public bool CanKillAny(Card attackerOrBlocker)
    {
      var attacker = FindAttacker(attackerOrBlocker);

      if (attacker != null)
      {
        return attacker.CanKillAnyBlocker();
      }

      var blocker = FindBlocker(attackerOrBlocker);

      if (blocker != null)
      {
        return blocker.CanKillAttacker();
      }

      return false;
    }
  }
}