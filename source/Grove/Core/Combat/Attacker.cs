namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Events;
  using Grove.AI;
  using Grove.Infrastructure;

  public class Attacker : GameObject, IHashable
  {    
    private readonly TrackableList<Blocker> _blockers = new TrackableList<Blocker>();
    private readonly Card _card;
    private readonly Card _planeswalker;
    private readonly Trackable<bool> _isBlocked = new Trackable<bool>();

    public Attacker(Card card, Card planeswalker, Game game)
    {
      Game = game;
      _card = card;
      _planeswalker = planeswalker;

      _blockers.Initialize(ChangeTracker);      
      _isBlocked.Initialize(ChangeTracker);
    }

    private Attacker() {}

    public IEnumerable<Blocker> Blockers { get { return _blockers; } }
    public int BlockersCount { get { return _blockers.Count; } }
    public IEnumerable<Blocker> BlockersInDamageAssignmentOrder { get { return _blockers.OrderBy(x => x.DamageAssignmentOrder); } }
    public Card Card { get { return _card; } }
    public Card Planeswalker { get { return _planeswalker; } }
    public Player Controller { get { return _card.Controller; } }
    public bool HasDeathTouch { get { return _card.Has().Deathtouch; } }
    public bool HasTrample { get { return _card.Has().Trample; } }
    public int LifepointsLeft { get { return _card.Life; } }
    public bool AssignsDamageAsThoughItWasntBlocked { get { return _card.Has().AssignsDamageAsThoughItWasntBlocked; } }
    public bool IsBlocked { get { return _isBlocked.Value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_isBlocked),
        calc.Calculate(_card),
        calc.Calculate(_blockers));
    }

    public void AddBlocker(Blocker blocker)
    {
      _blockers.Add(blocker);
      _isBlocked.Value = true;
    }    

    public bool CanBeBlockedBy(Card creature)
    {
      return _card.CanBeBlockedBy(creature);
    }
            
    public int CalculateDefendingPlayerLifeloss()
    {
      return QuickCombat.CalculateDefendingPlayerLifeloss(_card, _blockers.Select(x => x.Card));
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
      Publish(new RemovedFromCombatEvent(Card));

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
      return attacker != null ? attacker._card : null;
    }

    public bool CanBeDealtLeathalCombatDamage()
    {
      return QuickCombat.CanAttackerBeDealtLeathalDamage(Card, _blockers.Select(x => x.Card));
    }

    public bool CanKillAnyBlocker()
    {
      return QuickCombat.CanAttackerKillAnyBlocker(Card, _blockers.Select(x => x.Card));
    }

   
  }
}