namespace Grove.Core.Triggers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class OnBlock : Trigger, IReceive<BlockerJoinedCombat>
  {
    public bool GetsBlocked;
    public bool Blocks;
    
    public void Receive(BlockerJoinedCombat message)
    {            
      if (GetsBlocked && message.Attacker.Card == Ability.OwningCard)
      {
        Set();
        
      }

      else if (Blocks && message.Blocker.Card == Ability.OwningCard)
      {
        Set();
      }
    }
  }
}