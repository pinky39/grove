namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnBlock : Trigger, IReceive<BlockerJoinedCombat>, IReceive<StepStarted>
  {
    private readonly bool _becomesBlocked;
    private readonly bool _blocks;
    private readonly bool _triggerForEveryCreature;
    private readonly Trackable<int> _count = new Trackable<int>();

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
        _count.Value += 1;

        if (_triggerForEveryCreature || _count.Value == 1)
        {
          Set(message);
        }
      }

      else if (_blocks && message.Blocker.Card == Ability.OwningCard)
      {
        Set(message);
      }
    }

    public void Receive(StepStarted message)
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