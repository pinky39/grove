namespace Grove.Core.Cards.Triggers
{
  using Grove.Infrastructure;
  using Grove.Core.Messages;

  public class GetsBlocked : Trigger, IReceive<BlockerJoinedCombat>
  {
    public void Receive(BlockerJoinedCombat message)
    {
      if(message.Attacker.Card == Ability.OwningCard)
      {
        Set();
      }
    }
  }
}