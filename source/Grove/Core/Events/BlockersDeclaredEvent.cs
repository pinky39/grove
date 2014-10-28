namespace Grove.Events
{
  using System.Collections.Generic;

  public class BlockersDeclaredEvent
  {
    public readonly IEnumerable<Blocker> Blockers;
    
    public BlockersDeclaredEvent(IEnumerable<Blocker> blockers)
    {
      Blockers = blockers;
    }
  }
}