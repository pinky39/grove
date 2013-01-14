namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;
  using Messages;

  [Copyable]
  public class Combat : IHashable
  {
    private readonly TrackableList<Attacker> _attackers;
    private readonly TrackableList<Blocker> _blockers;
    private readonly Game _game;

    private Combat() {}

    public Combat(Game game)
    {
      _game = game;
      _attackers = new TrackableList<Attacker>(game.ChangeTracker);
      _blockers = new TrackableList<Blocker>(game.ChangeTracker);
    }

    public IEnumerable<Attacker> Attackers { get { return _attackers; } }
    private Player DefendingPlayer { get { return _game.Players.Defending; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_attackers),
        calc.Calculate(_blockers));
    }

    public void AssignCombatDamage(bool firstStrike = false)
    {
      IEnumerable<Blocker> blockers = firstStrike
        ? _blockers.Where(x => x.Card.HasFirstStrike)
        : _blockers.Where(x => x.Card.HasNormalStrike);

      IEnumerable<Attacker> attackers = firstStrike
        ? _attackers.Where(x => x.Card.HasFirstStrike)
        : _attackers.Where(x => x.Card.HasNormalStrike);

      foreach (Blocker blocker in blockers)
      {
        blocker.DistributeDamageToAttacker();
      }

      foreach (Attacker attacker in attackers)
      {
        attacker.DistributeDamageToBlockers();
      }
    }

    public bool CanAnyAttackerBeBlockedByAny(IEnumerable<Card> creatures)
    {
      foreach (Attacker attacker in _attackers)
      {
        foreach (Card creature in creatures)
        {
          if (attacker.CanBeBlockedBy(creature))
            return true;
        }
      }

      return false;
    }

    public void DealAssignedDamage()
    {
      foreach (Attacker attacker in _attackers)
      {
        attacker.DealAssignedDamage();
      }

      foreach (Blocker blocker in _blockers)
      {
        blocker.DealAssignedDamage();
      }

      DefendingPlayer.DealAssignedDamage();
    }

    private Attacker CreateAttacker(Card card)
    {
      return new Attacker(card, _game);
    }

    private Blocker CreateBlocker(Card blocker, Attacker attacker)
    {
      return new Blocker(blocker, attacker, _game);
    }

    public void JoinAttack(Card card, bool wasDeclared = false)
    {
      Attacker attacker = CreateAttacker(card);
      _attackers.Add(attacker);

      if (!card.Has().Vigilance)
        card.Tap();

      _game.Publish(new AttackerJoinedCombat
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
      Attacker attacker = FindAttacker(cardAttacker);
      Blocker blocker = CreateBlocker(cardBlocker, attacker);

      attacker.AddBlocker(blocker);
      _blockers.Add(blocker);

      _game.Publish(new BlockerJoinedCombat
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
      Attacker attacker = FindAttacker(card);

      if (attacker != null)
      {
        _attackers.Remove(attacker);
        attacker.RemoveFromCombat();
        return;
      }

      Blocker blocker = FindBlocker(card);

      if (blocker != null)
      {
        _blockers.Remove(blocker);
        blocker.RemoveFromCombat();
      }
    }

    public void RemoveAll()
    {
      foreach (Blocker blocker in _blockers)
      {
        blocker.RemoveFromCombat();
      }

      foreach (Attacker attacker in _attackers)
      {
        attacker.RemoveFromCombat();
      }

      _blockers.Clear();
      _attackers.Clear();
    }

    public void SetDamageAssignmentOrder()
    {
      foreach (Attacker attacker in _attackers)
      {
        Attacker attackerCopy = attacker;

        _game.Enqueue<SetDamageAssignmentOrder>(
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
      Attacker attacker = FindAttacker(card);

      if (attacker != null)
      {
        return attacker.CanBeDealtLeathalCombatDamage();
      }

      Blocker blocker = FindBlocker(card);

      if (blocker != null)
      {
        return blocker.WillBeDealtLeathalCombatDamage();
      }

      return false;
    }

    public bool HasBlockers(Card card)
    {
      Attacker attacker = FindAttacker(card);
      return attacker.BlockersCount > 0;
    }

    public bool CanBlockAtLeastOneAttacker(Card card)
    {
      return Attackers.Any(attacker => attacker.CanBeBlockedBy(card));
    }

    public int CountHowManyThisCouldBlock(Card card)
    {
      Player opponent = _game.Players.GetOpponent(card.Controller);
      return opponent.Battlefield.CreaturesThatCanAttack.Count(x => x.CanBeBlockedBy(card));
    }

    public bool CouldBeBlockedByAny(Card card)
    {
      Player opponent = _game.Players.GetOpponent(card.Controller);
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
      Attacker attacker = FindBlocker(blocker).Attacker;
      return attacker != null ? attacker.Card : null;
    }

    public bool CanKillAny(Card attackerOrBlocker)
    {
      Attacker attacker = FindAttacker(attackerOrBlocker);

      if (attacker != null)
      {
        return attacker.CanKillAnyBlocker();
      }

      Blocker blocker = FindBlocker(attackerOrBlocker);

      if (blocker != null)
      {
        return blocker.CanKillAttacker();
      }

      return false;
    }
  }
}