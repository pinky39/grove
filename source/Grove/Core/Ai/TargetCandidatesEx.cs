namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;

  public static class TargetCandidatesEx
  {
    public static IEnumerable<ITarget> RestrictController(this IEnumerable<ITarget> candidates, Player controller)
    {
      return candidates.Where(x => x.Card().Controller == controller);
    }
  }
}