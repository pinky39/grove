namespace Grove.Gameplay
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;
  using Messages;
  using Misc;

  [Copyable]
  public class Combat : GameObject, IHashable
  {
    private readonly TrackableList<Attacker> _attackers = new TrackableList<Attacker>();
    private readonly TrackableList<Blocker> _blockers = new TrackableList<Blocker>();

    public IEnumerable<Attacker> Attackers { get { return _attackers; } }
    public int AttackerCount { get { return _attackers.Count; } }
    private Player DefendingPlayer { get { return Players.Defending; } }
    public int BlockersCount { get { return _blockers.Count; }}
      
    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_attackers),
        calc.Calculate(_blockers));
    }

    public void Initialize(Game game)
    {
      Game = game;

      _attackers.Initialize(game.ChangeTracker);
      _blockers.Initialize(game.ChangeTracker);
    }

    public void AssignCombatDamage(bool firstStrike = false)
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
        attacker.DistributeDamageToBlockers();
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

      DefendingPlayer.DealAssignedDamage();
    }

    private Attacker CreateAttacker(Card card)
    {
      return new Attacker(card, Game);
    }

    private Blocker CreateBlocker(Card blocker, Attacker attacker)
    {
      return new Blocker(blocker, attacker, Game);
    }

    public void JoinAttack(Card card, bool wasDeclared = false)
    {
      var attacker = CreateAttacker(card);
      _attackers.Add(attacker);

      if (!card.Has().Vigilance)
        card.Tap();

      Publish(new AttackerJoinedCombat
        {
          Attacker = attacker,
          WasDeclared = wasDeclared
        });
    }

    public void DeclareAttacker(Card card)
    {
      if (!card.CanAttack)
        return;

      JoinAttack(card, wasDeclared: true);
    }

    public void DeclareBlocker(Card cardBlocker, Card cardAttacker)
    {
      var attacker = FindAttacker(cardAttacker);
      var blocker = CreateBlocker(cardBlocker, attacker);

      attacker.AddBlocker(blocker);
      _blockers.Add(blocker);

      Publish(new BlockerJoinedCombat
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

    public void SetDamageAssignmentOrder()
    {
      foreach (var attacker in _attackers)
      {
        var attackerCopy = attacker;

        Enqueue<SetDamageAssignmentOrder>(
          controller: attacker.Controller,
          init: p => p.Attacker = attackerCopy);
      }
    }

    private Attacker FindAttacker(Card cardAttacker)
    {
      return _attackers.FirstOrDefault(a => a.Card == cardAttacker);
    }

    public Blocker FindBlocker(Card cardBlocker)
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

      if (attacker == null)
        return false;

      return attacker.BlockersCount > 0;
    }

    public bool CanBlockAtLeastOneAttacker(Card card)
    {
      return Attackers.Any(attacker => attacker.CanBeBlockedBy(card));
    }

    public int CountHowManyThisCouldBlock(Card card)
    {
      var opponent = card.Controller.Opponent;
      return opponent.Battlefield.CreaturesThatCanAttack.Count(x => x.CanBeBlockedBy(card));
    }

    public bool CouldBeBlockedByAny(Card card)
    {
      var opponent = card.Controller.Opponent;
      return opponent.Battlefield.CreaturesThatCanBlock.Any(card.CanBeBlockedBy);
    }

    public bool WillAnyAttackerDealDamageToDefender()
    {
      return FindAttackerWhichWillDealGreatestDamageToDefender() != null;
    }

    public int EvaluateDamageAttackerWillDealToDefender(Card attacker)
    {
      return FindAttacker(attacker).CalculateDefendingPlayerLifeloss();
    }

    public Card FindAttackerWhichWillDealGreatestDamageToDefender(Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      return Attackers
        .Where(x => filter(x))
        .Select(x => new
          {
            Attacker = x,
            Damage = x.CalculateDefendingPlayerLifeloss()
          })
        .Where(x => x.Damage > 0)
        .OrderByDescending(x => x.Damage)
        .Select(x => x.Attacker)
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