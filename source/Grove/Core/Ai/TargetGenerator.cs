namespace Grove.Core.Ai
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class TargetGenerator : IEnumerable<ITarget>
  {    
    private readonly List<ITarget> _targets;

    public TargetGenerator(TargetSelector selector, Players players, Zones.Stack stack, int? maxX,
                           bool forcePickIfAnyValid = false)
    {      
      _targets = GetValidTargets(selector, players, stack, maxX, forcePickIfAnyValid);
    }

    public IEnumerator<ITarget> GetEnumerator()
    {
      return _targets.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private static List<ITarget> GetValidTargets(TargetSelector specification, Players players, 
      Zones.Stack stack, int? maxX, bool forcePickIfAnyValid)
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

      if (accepted.Count() == 0 && forcePickIfAnyValid)
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