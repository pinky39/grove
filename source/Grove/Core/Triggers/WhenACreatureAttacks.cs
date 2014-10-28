namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class WhenACreatureAttacks : Trigger, IReceive<AttackerJoinedCombatEvent>
  {
    private readonly Func<Parameters, bool> _predicate;

    private WhenACreatureAttacks() {}

    public WhenACreatureAttacks(Func<Parameters, bool> predicate = null)
    {
      _predicate = predicate ?? delegate { return true; };
    }

    public void Receive(AttackerJoinedCombatEvent e)
    {
      if (_predicate(new Parameters(e, this)))
      {
        Set(e);
      }
    }

    public class Parameters
    {
      private readonly AttackerJoinedCombatEvent _e;
      private readonly WhenACreatureAttacks _trigger;


      public Parameters(AttackerJoinedCombatEvent e, WhenACreatureAttacks trigger)
      {
        _e = e;
        _trigger = trigger;
      }

      public bool You { get { return _e.Attacker.Controller != _trigger.OwningCard.Controller; } }
      public bool Opponent { get { return !You; } }

      public bool AttackerHas(Func<Card, bool> predicate)
      {
        return predicate(_e.Attacker.Card);
      }
    }
  }
}