namespace Grove.Events
{
  public class AttackerJoinedCombatEvent
  {
    public AttackerJoinedCombatEvent(Attacker attacker, bool wasDeclared)
    {
      Attacker = attacker;
      WasDeclared = wasDeclared;
    }

    public readonly Attacker Attacker;
    public readonly bool WasDeclared;
  }
}