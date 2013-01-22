namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Targeting;

  public abstract class TargetingRule : MachinePlayRule
  {
    public override void Process(Ai.ActivationContext context)
    {
      var candidates = context.Selector.GenerateCandidates(Game.GenerateTargets);

      var parameters = new TargetingRuleParameters(candidates, context, Game);

      var targetsCombinations = SelectTargets(parameters)
        .Take(Search.MaxTargetCandidates)
        .ToList();

      if (targetsCombinations.Count < parameters.MinTargetCount())
      {
        if (parameters.CanCancel)
        {
          context.CancelActivation = true;
          return;
        }

        targetsCombinations = ForceSelectTargets(parameters)
          .Take(Search.MaxTargetCandidates)
          .ToList();
      }


      context.Targets = targetsCombinations;
    }

    protected abstract IEnumerable<Targets> SelectTargets(TargetingRuleParameters p);

    protected virtual IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      return SelectTargets(p);
    }

    protected IEnumerable<T> None<T>()
    {
      yield break;
    }

    protected IList<Targets> Group(IEnumerable<Card> candidates, int targetCount, Action<ITarget, Targets> add = null)
    {
      return Group(candidates.Cast<ITarget>().ToList(), targetCount, add);
    }

    protected IList<Targets> Group(IEnumerable<Player> candidates, int targetCount, Action<ITarget, Targets> add = null)
    {
      return Group(candidates.Cast<ITarget>().ToList(), targetCount, add);
    }

    protected IList<Targets> Group(IEnumerable<Effect> candidates, int targetCount, Action<ITarget, Targets> add = null)
    {
      return Group(candidates.Cast<ITarget>().ToList(), targetCount, add);
    }

    protected IList<Targets> Group(IList<ITarget> candidates1, IList<ITarget> candidates2,
      Action<ITarget, Targets> add = null)
    {
      var results = new List<Targets>();

      if (candidates1.Count == 0 || candidates2.Count == 0)
        return results;

      add = add ?? ((trg, trgts) => trgts.AddEffect(trg));

      var index1 = 0;
      var index2 = 0;

      var groupCount = Math.Max(candidates1.Count, candidates2.Count);

      for (var i = 0; i < groupCount; i++)
      {
        // generate some options by mixing candidates 
        // from 2 selectors

        var targets = new Targets();

        add(candidates1[index1], targets);
        add(candidates2[index2], targets);

        index1++;
        index2++;

        if (index1 == candidates1.Count)
          index1 = 0;

        if (index2 == candidates2.Count)
          index2 = 0;

        results.Add(targets);
      }

      return results;
    }

    protected IList<Targets> Group(IList<ITarget> candidates, int targetCount, Action<ITarget, Targets> add = null)
    {
      var results = new List<Targets>();

      if (candidates.Count < targetCount)
        return results;

      add = add ?? ((trg, trgts) => trgts.AddEffect(trg));

      // generate possible groups by varying only the last element
      // since the number of different groups tried will be small
      // this is a reasonable approximation.
      var groupCount = candidates.Count - targetCount + 1;

      for (var i = 0; i < groupCount; i++)
      {
        var targets = new Targets();

        // add first targetCount - 1
        for (var j = 0; j < targetCount - 1; j++)
        {
          add(candidates[j], targets);
        }

        // add last
        add(candidates[targetCount - 1 + i], targets);

        results.Add(targets);
      }

      return results;
    }

    protected int CalculateAttackerScore(Card card)
    {
      return Combat.CouldBeBlockedByAny(card) ? 5 : 0 + card.CalculateCombatDamage(allDamageSteps: true);
    }

    protected int CalculateBlockerScore(Card card)
    {
      var count = Combat.CountHowManyThisCouldBlock(card);

      if (count > 0)
      {
        return count*10 + card.Toughness.GetValueOrDefault();
      }

      return 0;
    }

    protected IEnumerable<Card> GetCandidatesForAttackerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetingRuleParameters p)
    {
      return p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsAttacker)
        .Select(
          x =>
            new
              {
                Card = x,
                Gain =
                  QuickCombat.CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(
                    attacker: x,
                    blockers: Combat.GetBlockers(x),
                    powerIncrease: powerIncrease.Value,
                    toughnessIncrease: toughnessIncrease.Value)
              })
        .Where(x => x.Gain > 0)
        .OrderByDescending(x => x.Gain)
        .Select(x => x.Card);
    }

    protected IEnumerable<Card> GetCandidatesForBlockerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetingRuleParameters p)
    {
      return p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsBlocker)
        .Select(
          x =>
            new
              {
                Card = x,
                Gain =
                  QuickCombat.CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(
                    blocker: x,
                    attacker: Combat.GetAttacker(x),
                    powerIncrease: powerIncrease.Value,
                    toughnessIncrease: toughnessIncrease.Value)
              })
        .Where(x => x.Gain > 0)
        .OrderByDescending(x => x.Gain)
        .Select(x => x.Card);
    }
  }
}