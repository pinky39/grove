namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;
  using States;

  public class OnEndOfCombatIfAttackedOrBlocked : Trigger, IReceive<AttackerJoinedCombat>, IReceive<BlockerJoinedCombat>, IReceive<StepStarted>
  {
    private readonly Trackable<bool> _hasAttackedOrBlocked = new Trackable<bool>();
    
    public void Receive(AttackerJoinedCombat message)
    {
      if (message.Attacker.Card == OwningCard)
        _hasAttackedOrBlocked.Value = true;
    }

    public void Receive(BlockerJoinedCombat message)
    {
      if (message.Blocker.Card == OwningCard)
        _hasAttackedOrBlocked.Value = true;
    }

    public void Receive(StepStarted message)
    {
      if (message.Step == Step.EndOfCombat && _hasAttackedOrBlocked == true)
      {        
        _hasAttackedOrBlocked.Value = false;
        
        Set();                
      }              
    }

    protected override void Initialize()
    {
      _hasAttackedOrBlocked.Initialize(ChangeTracker);
    }
  }
}