namespace Grove.Core.Details.Cards.Triggers
{
  using Infrastructure;
  using Messages;

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