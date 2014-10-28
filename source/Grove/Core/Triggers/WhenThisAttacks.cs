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
      if (!OwningCard.IsAttacker)
        return;                  

      if (_predicate(new Parameters()))
      {
        Set();
      }
    }

    public class Parameters {}
  }
}