namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public static class TargetCandidatesEx
  {
    public static IEnumerable<Target> RestrictController(this IEnumerable<Target> candidates, IPlayer controller)
    {
      return candidates
        .Where(x => (x.IsCard() && x.Card().Controller == controller) ||
          (x.IsEffect() && x.Effect().Controller == controller));
    }
  }
}