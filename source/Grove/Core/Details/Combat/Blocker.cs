namespace Grove.Core.Details.Combat
{
  using System.Linq;
  using Ai;
  using Infrastructure;
  using Messages;

  [Copyable]
  public class Blocker : IHashable
  {
    private readonly TrackableList<Damage> _assignedDamage;
    private readonly Trackable<Attacker> _attacker;
    private readonly ChangeTracker _changeTracker;
    private readonly Trackable<int> _damageAssignmentOrder;
    private readonly Publisher _publisher;

    private Blocker() {}

    private Blocker(Card card, Attacker attacker, ChangeTracker changeTracker, Publisher publisher)
    {
      Card = card;
      _attacker = new Trackable<Attacker>(attacker, changeTracker);
      _assignedDamage = new TrackableList<Damage>(changeTracker);
      _damageAssignmentOrder = new Trackable<int>(changeTracker);
      _changeTracker = changeTracker;
      _publisher = publisher;
    }

    public Attacker Attacker { get { return _attacker.Value; } private set { _attacker.Value = value; } }
    public Card Card { get; private set; }
    public Player Controller { get { return Card.Controller; } }

    public int DamageAssignmentOrder { get { return _damageAssignmentOrder.Value; } set { _damageAssignmentOrder.Value = value; } }

    public bool HasAssignedLeathalDamage
    {
      get
      {
        return Card.HasLeathalDamage ||
          _assignedDamage.Sum(x => x.Amount) + Card.Damage >= Card.Toughness ||
            _assignedDamage.Any(x => x.IsLeathal);
      }
    }

    private bool HasAttacker { get { return Attacker != null; } }

    public int LifepointsLeft { get { return Card.LifepointsLeft; } }
    public int Score { get { return ScoreCalculator.CalculatePermanentScore(Card); } }
    public int DamageThisWillDealInOneDamageStep { get { return Card.Power.Value; } }
    public int Toughness { get { return Card.Toughness.Value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(Card),
        DamageAssignmentOrder,
        calc.Calculate(_assignedDamage));
    }

    public void AssignDamage(Damage damage)
    {
      _assignedDamage.Add(damage);
    }

    public void ClearAssignedDamage()
    {
      _assignedDamage.Clear();
    }

    public void DealAssignedDamage()
    {
      foreach (var damage in _assignedDamage)
      {
        Card.DealDamage(damage);
      }

      ClearAssignedDamage();
    }

    public void DistributeDamageToAttacker()
    {
      if (Attacker != null)
      {
        var damage = new Damage(
          source: Card,
          amount: DamageThisWillDealInOneDamageStep,
          isCombat: true,
          changeTracker: _changeTracker
          );

        Attacker.AssignDamage(damage);
      }
    }

    public void RemoveAttacker()
    {
      Attacker = null;
    }

    public void RemoveFromCombat()
    {
      _publisher.Publish(new RemovedFromCombat {Card = Card});

      if (HasAttacker)
      {
        Attacker.RemoveBlocker(this);
        Attacker = null;
      }
    }

    public bool WillBeDealtLeathalCombatDamage()
    {
      if (Attacker == null)
        return false;

      return Core.Combat.CanBlockerBeDealtLeathalCombatDamage(Card, Attacker.Card);
    }

    public int CalculateGainIfGivenABoost(int power, int toughness)
    {
      if (Attacker == null)
      {
        return 0;
      }

      var withoutBoost = Core.Combat.CanBlockerBeDealtLeathalCombatDamage(Card, Attacker);
      if (!withoutBoost)
        return 0;

      var withBoost = Core.Combat.CanBlockerBeDealtLeathalCombatDamage(Card, Attacker, power, toughness);
      return !withBoost ? Card.Score : 1;
    }

    public Card GetBestDamagePreventionCandidate()
    {
      if (WillBeDealtLeathalCombatDamage())
      {
        return Attacker.Card;
      }
      return null;
    }

    [Copyable]
    public class Factory : IBlockerFactory
    {
      private readonly ChangeTracker _changeTracker;
      private readonly Publisher _publisher;

      private Factory() {}

      public Factory(ChangeTracker changeTracker, Publisher publisher)
      {
        _changeTracker = changeTracker;
        _publisher = publisher;
      }

      public Blocker Create(Card blocker, Attacker attacker)
      {
        return new Blocker(blocker, attacker, _changeTracker, _publisher);
      }
    }
  }
}