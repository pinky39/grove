namespace Grove.Core.Cards.Triggers
{
  using Infrastructure;
  using Messages;

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