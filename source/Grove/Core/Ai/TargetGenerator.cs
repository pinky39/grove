namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class TargetGenerator : IEnumerable<ITarget>
  {
    private readonly bool _forcePickIfAnyValid;
    private readonly List<ITarget> _targets;

    public TargetGenerator(TargetSelector selector, Players players, Zones.Stack stack, int? maxX, int maxTargets,
                           bool forcePickIfAnyValid = false)
    {
      _forcePickIfAnyValid = forcePickIfAnyValid;
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

    private List<ITarget> GetValidTargets(TargetSelector specification,
                                          Players players, Zones.Stack stack, int? maxX, int maxTargets)
    {
      IEnumerable<TargetCandidate> selected;

      var all =
        players.SelectMany(player => player.GetTargets(specification)).Concat(
          stack.Where(specification.IsValid))
          .Select(target =>
            new TargetCandidate
              {
                Target = target,
                Score = specification.CalculateScore(target, maxX)
              })
          .ToList();

      var accepted = all.Where(x => x.Score != WellKnownTargetScores.NotAccepted);

      if (accepted.Count() == 0 && _forcePickIfAnyValid)
      {
        // if there are no targets with good scores select
        // all
        selected = all;
      }
      else
      {
        selected = accepted
          .OrderByDescending(x => x.Score);
      }

      return selected
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