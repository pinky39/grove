namespace Grove.Events
{
  public class AttackerJoinedCombatEvent
  {
    public readonly Attacker Attacker;

    public AttackerJoinedCombatEvent(Attacker attacker)
    {
      Attacker = attacker;
    }
  }
}