namespace Grove
{
  using System.Linq;
  using Events;
  using Grove.AI;
  using Grove.Infrastructure;

  public class Blocker : GameObject, IHashable
  {    
    private readonly Trackable<Attacker> _attacker;
    private readonly Trackable<int> _damageAssignmentOrder = new Trackable<int>();

    private Blocker() {}

    public Blocker(Card card, Attacker attacker, Game game)
    {
      Card = card;
      Game = game;

      _attacker = new Trackable<Attacker>(attacker);
      _attacker.Initialize(ChangeTracker);
      
      _damageAssignmentOrder.Initialize(ChangeTracker);
    }

    public Attacker Attacker { get { return _attacker.Value; } private set { _attacker.Value = value; } }
    public Card Card { get; private set; }
    public Player Controller { get { return Card.Controller; } }

    public int DamageAssignmentOrder { get { return _damageAssignmentOrder.Value; } set { _damageAssignmentOrder.Value = value; } }    

    private bool HasAttacker { get { return Attacker != null; } }

    public int LifepointsLeft { get { return Card.Life; } }
    public int Score { get { return ScoreCalculator.CalculatePermanentScore(Card); } }
    public int Toughness { get { return Card.Toughness.Value; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(Card),
        DamageAssignmentOrder);
    }                

    public void RemoveAttacker()
    {
      Attacker = null;
    }

    public void RemoveFromCombat()
    {
      Publish(new RemovedFromCombatEvent(Card));

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

      return QuickCombat.CanBlockerBeDealtLeathalCombatDamage(Attacker, Card);
    }

    public bool CanKillAttacker()
    {
      if (Attacker == null)
        return false;

      return QuickCombat.CanAttackerBeDealtLeathalDamage(Attacker.Card, Card.ToEnumerable());
    }
  }
}