namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnBlock : Trigger, IReceive<BlockerJoinedCombat>
  {
    private readonly bool _becomesBlocked;
    private readonly bool _blocks;
    private readonly bool _triggerForEveryCreature;    

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
        if (_triggerForEveryCreature || message.Attacker.BlockersCount == 1)
        {                    
          Set();
        }
      }

      else if (_blocks && message.Blocker.Card == Ability.OwningCard)
      {
        Set();
      }
    }   
  }
}