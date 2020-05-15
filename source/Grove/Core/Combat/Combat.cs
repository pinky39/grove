namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Events;
  using Infrastructure;

  [Copyable]
  public class Combat : GameObject, IHashable
  {
    private readonly TrackableList<Attacker> _attackers = new TrackableList<Attacker>();
    private readonly TrackableList<Blocker> _blockers = new TrackableList<Blocker>();
    private readonly TrackableList<AssignedDamage> _assignedDamage = new TrackableList<AssignedDamage>();

    public IEnumerable<Attacker> Attackers { get { return _attackers; } }
    public IEnumerable<Blocker> Blockers { get { return _blockers; } }
    public int AttackerCount { get { return _attackers.Count; } }
    public int BlockersCount { get { return _blockers.Count; } }

    private Player DefendingPlayer { get { return Players.Defending; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_attackers),
        calc.Calculate(_blockers));
    }

    public void Initialize(Game game)
    {
      Game = game;

      _attackers.Initialize(ChangeTracker);
      _blockers.Initialize(ChangeTracker);
      _assignedDamage.Initialize(ChangeTracker);
    }

    public void DistributeCombatDamage(bool firstStrike = false)
    {
      var blockers = (firstStrike
        ? _blockers.Where(x => x.Card.HasFirstStrike)
        : _blockers.Where(x => x.Card.HasNormalStrike))
        .ToList();

      var attackers = (firstStrike
        ? _attackers.Where(x => x.Card.HasFirstStrike)
        : _attackers.Where(x => x.Card.HasNormalStrike))
        .ToList();

      foreach (var blocker in blockers)
      {
        if (blocker.Attacker == null)
          continue;

        _assignedDamage.Add(new AssignedDamage(
          blocker.Card.CalculateCombatDamageAmount(), 
          blocker.Card, 
          blocker.Attacker.Card));                
      }

      foreach (var attacker in attackers)
      {
        Enqueue(new AssignCombatDamage(
          Players.Attacking,
            attacker));                
      }
    }

    public void AssignDamageToBlockers(Attacker attacker, DamageDistribution distribution)
    {
       foreach (var blocker in attacker.Blockers)
      {
        _assignedDamage.Add(new AssignedDamage(
          distribution[blocker], 
          blocker.Attacker.Card,
          blocker.Card));
      }
      
      if (attacker.HasTrample || attacker.AssignsDamageAsThoughItWasntBlocked || attacker.IsBlocked == false)
      {
        _assignedDamage.Add(new AssignedDamage(
          amount: attacker.Card.CalculateCombatDamageAmount() - distribution.Total,
          source: attacker.Card,
          target: attacker.Planeswalker ?? (IDamageable) Players.Defending));                
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
      foreach (var assignedDamage in _assignedDamage)
      {
        assignedDamage.Source.DealDamageTo(
          assignedDamage.Amount, 
          assignedDamage.Target, 
          isCombat: true);
      }
      
      _assignedDamage.Clear();
    }

    public void AddAttacker(Card card, Card planeswalker)
    {
      var attacker = CreateAttacker(card, planeswalker);
      _attackers.Add(attacker);

      if (!card.Has().Vigilance)
        card.Tap();

      Publish(new AttackerJoinedCombatEvent(attacker));
    }

    public void AddBlocker(Card cardBlocker, Card cardAttacker)
    {
      var attacker = FindAttacker(cardAttacker);
      var blocker = CreateBlocker(cardBlocker, attacker);

      attacker.AddBlocker(blocker);
      _blockers.Add(blocker);

      Publish(new BlockerJoinedCombatEvent(blocker, attacker));
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
        Enqueue(new SetDamageAssignmentOrder(
          attacker.Controller,
          attacker));
      }
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

    private Attacker CreateAttacker(Card card, Card planeswalker)
    {
      return new Attacker(card, planeswalker, Game);
    }

    private Blocker CreateBlocker(Card blocker, Attacker attacker)
    {
      return new Blocker(blocker, attacker, Game);
    }

    public Attacker FindAttacker(Card cardAttacker)
    {
      return _attackers.FirstOrDefault(a => a.Card == cardAttacker);
    }

    [Copyable]
    public class AssignedDamage
    {
      public readonly int Amount;
      public readonly Card Source;
      public readonly IDamageable Target;

      private AssignedDamage() { }

      public AssignedDamage(int amount, Card source, IDamageable target)
      {
        Amount = amount;
        Source = source;
        Target = target;
      }

      public bool IsLeathal { get { return Source.Has().Deathtouch; } }
    }
  }
}