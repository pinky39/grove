namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;

  public delegate IEnumerable<ITarget> TargetGeneratorDelegate();

  public class TargetSelectors : TargetBag<TargetSelector>
  {
    public bool NeedsTargets { get { return Count > 0; } }
    public TargetsFilterDelegate Filter { get; set; }

    public TargetCandidates GenerateCandidates(TargetGeneratorDelegate generator)
    {
      var all = new TargetCandidates();

      foreach (var name in Names)
      {
        var candidates = new List<TargetCandidate>();

        foreach (var target in generator())
        {
          if (this[name].IsValid(target))
          {
            candidates.Add(new TargetCandidate(target));
          }
        }

        all[name] = candidates;
      }

      return all;
    }

    public bool AreTargetsStillValid(Targets targets)
    {
      return targets.All(target => this[target.Key].IsValid(target.Value));
    }
  }
}