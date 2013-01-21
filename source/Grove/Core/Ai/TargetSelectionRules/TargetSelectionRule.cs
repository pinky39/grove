namespace Grove.Core.Ai.TargetSelectionRules
{
  using System.Collections.Generic;
  using Decisions.Results;
  using Targeting;
  using System.Linq;

  public abstract class TargetSelectionRule : MachinePlayRule
  {    
    public override IList<Playable> Process(IEnumerable<Playable> playables, ActivationPrerequisites prerequisites)
    {
      var results = new List<Playable>();

      var candidates = prerequisites.Selector.GenerateCandidates(Game.GenerateTargets);

      var parameters = new TargetSelectionParameters(candidates, Game);
      
      var targetsCombinations = SelectTargets(parameters)
        .Take(Search.MaxTargetCandidates)
        .ToList();

      foreach (var playable in playables)
      {
        var replicas = playable.Replicate(targetsCombinations.Count);

        for (var index = 0; index < targetsCombinations.Count; index++)
        {
          var targets = targetsCombinations[index];
          var replica = replicas[index];

          replica.ActivationParameters.Targets = targets;
          results.Add(replica);
        }
      }

      return results;
    }    

    protected abstract IEnumerable<Targets> SelectTargets(TargetSelectionParameters parameters);
  }
}