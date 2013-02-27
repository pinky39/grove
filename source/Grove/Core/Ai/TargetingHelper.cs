namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using Targeting;
  using TargetingRules;

  public class TargetingHelper
  {
    public static bool IsGoodTarget(ITarget target, Card owningCard, TargetSelector selector, IEnumerable<TargetingRule> rules)
    {
      var activation = new ActivationContext(owningCard, selector);

      foreach (TargetingRule rule in rules)
      {
        rule.Process(activation);
      }

      foreach (var targetsCombination in activation.TargetsCombinations())
      {
        if (targetsCombination.Targets.Effect.Contains(target))
          return true;
      }

      return false;   
    }

    public static IEnumerable<Targets> GenerateTargets(Card owningCard, TargetSelector selector, IEnumerable<TargetingRule> rules, bool force = false)
    {
      var activation = new ActivationContext(owningCard, selector);
      activation.CanCancel = !force;

      foreach (TargetingRule rule in rules)
      {
        rule.Process(activation);
      }

      foreach (var targetsCombination in activation.TargetsCombinations())
      {
        yield return targetsCombination.Targets;
      }

      yield break;
    }
  }
}