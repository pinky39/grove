namespace Grove.Events
{
  using System.Collections.Generic;

  public class AttackersDeclaredEvent
  {
    public readonly IEnumerable<Attacker> Attackers;
    
    public AttackersDeclaredEvent(IEnumerable<Attacker> attackers)
    {
      Attackers = attackers;
    }
  }
}