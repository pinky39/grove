namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;

  public static class TargetCandidatesEx
  {
    public static IEnumerable<TargetCandidate> RestrictController(this IEnumerable<TargetCandidate> candidates, Player controller)
    {
      return candidates.Where(x => TargetEx.Card(x.Target).Controller == controller);
    }
  }
}