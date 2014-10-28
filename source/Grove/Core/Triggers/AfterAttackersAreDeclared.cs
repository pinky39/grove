namespace Grove.Triggers
{
  using System;
  using System.Collections.Generic;
  using Events;
  using Infrastructure;

  public class AfterAttackersAreDeclared : Trigger, IReceive<AttackersDeclaredEvent>
  {
    private readonly Func<Parameters, bool> _predicate;

    private AfterAttackersAreDeclared() {}

    public AfterAttackersAreDeclared(Func<Parameters, bool> predicate)
    {
      _predicate = predicate ?? delegate { return true; };
    }

    public void Receive(AttackersDeclaredEvent e)
    {
      if (_predicate(new Parameters(e, this)))
      {
        Set(e);
      }
    }

    public class Parameters
    {
      private readonly AttackersDeclaredEvent _e;
      private readonly AfterAttackersAreDeclared _t;

      public Parameters(AttackersDeclaredEvent e, AfterAttackersAreDeclared t)
      {
        _e = e;
        _t = t;
      }

      public bool Yours { get { return _t.OwningCard.Controller.IsActive; } }
      public bool Opponents { get { return !Yours; } }
      public IEnumerable<Attacker> Attackers { get { return _e.Attackers; } }
    }
  }
}