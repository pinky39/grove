namespace Grove.Triggers
{
  using System;
  using Events;
  using Infrastructure;

  public class WhenThisBlocks : Trigger, IReceive<BlockerJoinedCombatEvent>
  {
    private readonly Func<Parameters, bool> _predicate;

    private WhenThisBlocks() {}

    public WhenThisBlocks(Func<Parameters, bool> predicate = null)
    {
      _predicate = predicate ?? delegate { return true; };
    }

    public void Receive(BlockerJoinedCombatEvent e)
    {
      if (e.Blocker.Card == Ability.OwningCard && _predicate(new Parameters(e)))
      {
        Set(e);
      }
    }

    public class Parameters
    {
      private readonly BlockerJoinedCombatEvent _e;

      public Parameters(BlockerJoinedCombatEvent e)
      {
        _e = e;
      }

      public bool AttackerHas(Func<Card, bool> predicate)
      {
        return predicate(_e.Attacker.Card);
      }
    }
  }
}