namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;
  using States;

  public class OnBlock : Trigger, IReceive<BlockerJoinedCombat>, IReceive<StepStarted>
  {
    private readonly bool _becomesBlocked;
    private readonly bool _blocks;
    private readonly bool _triggerForEveryCreature;
    private Trackable<int> _count = new Trackable<int>();

    private OnBlock() {}

    public OnBlock(bool becomesBlocked = false, bool blocks = false, bool triggerForEveryCreature = false)
    {
      _becomesBlocked = becomesBlocked;
      _blocks = blocks;
      _triggerForEveryCreature = triggerForEveryCreature;
    }

    public void Receive(BlockerJoinedCombat message)
    {
      if (_becomesBlocked && message.Attacker.Card == Ability.OwningCard)
      {
        _count.Value++;

        if (_triggerForEveryCreature || _count == 1)
        {
          Set();
        }
      }

      else if (_blocks && message.Blocker.Card == Ability.OwningCard)
      {
        Set();
      }
    }

    public void Receive(StepStarted message)
    {
      if (message.Step == Step.CombatDamage)
        _count.Value = 0;
    }

    protected override void Initialize()
    {
      _count.Initialize(ChangeTracker);
    }
  }
}