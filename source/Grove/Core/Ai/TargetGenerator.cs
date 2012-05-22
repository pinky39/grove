namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class TargetGenerator : IEnumerable<ITarget>
  {
    private readonly int _maxTargets;
    private readonly Players _players;
    private readonly Zones.Stack _stack;
    private readonly int? _maxX;
    private readonly List<TargetCandidate> _targets;

    public TargetGenerator(TargetSelector selector, Players players, Zones.Stack stack, int? maxX, int maxTargets)
    {
      _players = players;
      _stack = stack;
      _maxX = maxX;
      _maxTargets = maxTargets;
      _targets = GetValidTargets(selector);
    }

    public IEnumerator<ITarget> GetEnumerator()
    {
      var filtered = _targets
        .Where(x => x.Score != RankBounds.NotAcceptedRank);

      if (filtered.None())
        filtered = _targets;

      return filtered
        .OrderByDescending(x => x.Score)        
        .Take(_maxTargets)
        .Select(x => x.Target)
        .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private List<TargetCandidate> GetValidTargets(TargetSelector specification)
    {
      return
        _players.SelectMany(player => player.GetTargets(specification)).Concat(
        _stack.Where(specification.IsValid))
          .Select(target =>
            new TargetCandidate{
              Target = target,
              Score = specification.CalculateScore(target, _maxX)
            })
          .ToList();
    }

    private class TargetCandidate
    {
      public int Score { get; set; }
      public ITarget Target { get; set; }
    }
  }
}