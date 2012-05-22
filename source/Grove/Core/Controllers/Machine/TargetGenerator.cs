namespace Mtg.Core.Controllers.Machine
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Targets;

  public class TargetGenerator : IEnumerable<object>
  {
    private readonly int _maxTargets;
    private readonly List<TargetCandidate> _targets;

    public TargetGenerator(SpellTargetSelector spec, Game game, int maxTargets)
    {
      _maxTargets = maxTargets;
      _targets = GetValidTargets(spec, game);
    }

    public IEnumerator<object> GetEnumerator()
    {
      var filtered = _targets
        .Where(x => x.Score != RankBounds.NotAcceptedRank);
      
      if (filtered.None())
        filtered = _targets;
      
      return filtered
        .OrderBy(x => x.Score)
        .Take(_maxTargets)
        .Select(x => x.Target)
        .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private static List<TargetCandidate> GetValidTargets(SpellTargetSelector specification, Game game)
    {
      return
        game.Players
          .SelectMany(player => player.GetTargets(specification))
          .Select(target =>
            new TargetCandidate{
              Target = target,
              Score = specification.CalculateScore(target, game)
            })
          .ToList();
    }

    private class TargetCandidate
    {
      public int Score { get; set; }
      public object Target { get; set; }
    }
  }
}