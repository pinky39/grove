namespace Grove.Gameplay.Triggers
{
  using Infrastructure;
  using Messages;

  public class OnBlock : Trigger, IReceive<BlockerJoinedCombat>
  {
    private readonly bool _becomesBlocked;
    private readonly bool _blocks;

    private OnBlock() {}

    public OnBlock(bool becomesBlocked = false, bool blocks = false)
    {
      _becomesBlocked = becomesBlocked;
      _blocks = blocks;
    }

    public void Receive(BlockerJoinedCombat message)
    {
      if (_becomesBlocked && message.Attacker.Card == Ability.OwningCard)
      {
        Set();
      }

      else if (_blocks && message.Blocker.Card == Ability.OwningCard)
      {
        Set();
      }
    }
  }
}