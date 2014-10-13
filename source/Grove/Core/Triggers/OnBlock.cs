namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class OnBlock : Trigger, IReceive<BlockerJoinedCombatEvent>, IReceive<StepStartedEvent>
  {
    private readonly bool _becomesBlocked;
    private readonly bool _blocks;
    private readonly Trackable<int> _count = new Trackable<int>();
    private readonly bool _triggerForEveryCreature;
    private readonly Func<Card, bool> _attackerFilter;

    private OnBlock()
    {
    }

    public OnBlock(bool becomesBlocked = false, bool blocks = false, bool triggerForEveryCreature = false,
      Func<Card, bool> attackerFilter = null)
    {
      _becomesBlocked = becomesBlocked;
      _blocks = blocks;
      _triggerForEveryCreature = triggerForEveryCreature;

      _attackerFilter = attackerFilter ?? delegate { return true; };
    }

    public void Receive(BlockerJoinedCombatEvent message)
    {
      if (_becomesBlocked && message.Attacker.Card == Ability.OwningCard)
      {
        _count.Value += 1;

        if (_triggerForEveryCreature || _count.Value == 1)
        {
          Set(message);
        }
      }

      else if (_blocks && message.Blocker.Card == Ability.OwningCard && _attackerFilter(message.Attacker.Card))
      {
        Set(message);
      }
    }

    public void Receive(StepStartedEvent message)
    {
      if (message.Step == Step.EndOfCombat)
        _count.Value = 0;
    }

    protected override void Initialize()
    {
      _count.Initialize(ChangeTracker);
    }
  }
}