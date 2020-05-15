namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class WhenThisAttacks : Trigger, IReceive<AttackersDeclaredEvent>
  {
    private readonly Func<Parameters, bool> _predicate;

    private WhenThisAttacks() {}

    public WhenThisAttacks(Func<Parameters, bool> predicate = null)
    {
      _predicate = predicate ?? delegate { return true; };
    }

    public void Receive(AttackersDeclaredEvent e)
    {
      var attacker = Combat.FindAttacker(OwningCard);

      if (attacker == null)
        return;      
      
      if (_predicate(new Parameters { Attacker = attacker }))
      {
        Set();
      }
    }

    public class Parameters {
      public Attacker Attacker;
    }
  }
}