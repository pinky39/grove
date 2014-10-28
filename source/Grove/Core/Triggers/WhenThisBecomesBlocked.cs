namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class WhenThisBecomesBlocked : Trigger, IReceive<BlockerJoinedCombatEvent>, IReceive<StepStartedEvent>
  {
    private readonly Trackable<int> _count = new Trackable<int>();
    private readonly Func<Parameters, bool> _predicate;
    private readonly bool _triggerForEveryBlocker;

    private WhenThisBecomesBlocked()
    {      
    }
    
    public WhenThisBecomesBlocked(bool triggerForEveryBlocker, Func<Parameters, bool> predicate = null)
    {
      _triggerForEveryBlocker = triggerForEveryBlocker;
      _predicate = predicate ?? delegate { return true; };
    }

    public void Receive(BlockerJoinedCombatEvent message)
    {
      if (message.Attacker.Card != Ability.OwningCard || !_predicate(new Parameters()))
        return;

      _count.Value += 1;

      if (_triggerForEveryBlocker || _count.Value == 1)
      {
        Set(message);
      }
    }

    public void Receive(StepStartedEvent message)
    {
      if (message.Step == Step.EndOfCombat)
      {
        _count.Value = 0;
      }
    }

    protected override void Initialize()
    {
      _count.Initialize(ChangeTracker);
    }

    public class Parameters {}
  }  
}