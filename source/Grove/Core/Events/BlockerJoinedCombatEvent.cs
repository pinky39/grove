namespace Grove.Events
{
  public class BlockerJoinedCombatEvent
  {
    public readonly Attacker Attacker;
    public readonly Blocker Blocker;

    public BlockerJoinedCombatEvent(Blocker blocker, Attacker attacker)
    {
      Blocker = blocker;
      Attacker = attacker;
    }
  }
}