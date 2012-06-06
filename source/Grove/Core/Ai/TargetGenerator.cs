namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class TargetGenerator : IEnumerable<ITarget>
  {
    private readonly List<ITarget> _targets;

    public TargetGenerator(TargetSelector selector, Players players, Zones.Stack stack, int? maxX, int maxTargets)
    {
      _targets = GetValidTargets(selector, players, stack, maxX, maxTargets);
    }

    public IEnumerator<ITarget> GetEnumerator()
    {
      return _targets.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private static List<ITarget> GetValidTargets(TargetSelector specification, Players players, Zones.Stack stack, int? maxX,
                                          int maxTargets)
    {
      return
        players.SelectMany(player => player.GetTargets(specification)).Concat(
          stack.Where(specification.IsValid))
          .Select(target =>
                  new TargetCandidate
                    {
                      Target = target,
                      Score = specification.CalculateScore(target, maxX)
                    })
          .Where(x => x.Score != WellKnownTargetScores.NotAccepted)
          .OrderByDescending(x => x.Score)
          .Take(maxTargets)
          .Select(x => x.Target)
          .ToList();
    }

    private class TargetCandidate
    {
      public int Score { get; set; }
      public ITarget Target { get; set; }
    }
  }
}