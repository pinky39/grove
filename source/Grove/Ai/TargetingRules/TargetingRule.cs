﻿namespace Grove.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Effects;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public abstract class TargetingRule : MachinePlayRule
  {        
    public override void Process(Ai.ActivationContext c)
    {
      var candidates = c.Selector.GenerateCandidates(c.TriggerMessage);

      var parameters = new TargetingRuleParameters(candidates, c, Game);

      var targetsCombinations = SelectTargets(parameters)                
        .ToList();
      
      if (targetsCombinations.Count == 0)
      {
        if (c.CanCancel)
        {
          c.CancelActivation = true;
          return;
        }

        targetsCombinations = ForceSelectTargets(parameters)
          .Take(Search.MaxTargetCandidates)
          .ToList();
      }


      c.SetPossibleTargets(targetsCombinations);
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

    protected IList<Targets> Group(IEnumerable<Gameplay.Card.Card> candidates, int minTargetCount, int? maxTargetCount = null, Action<ITarget, Targets> add = null)
    {
      return Group(candidates.Cast<ITarget>().ToList(), minTargetCount, maxTargetCount, add);
    }

    protected IList<Targets> Group(IEnumerable<Player> candidates, int minTargetCount, int? maxTargetCount = null, Action<ITarget, Targets> add = null)
    {
      return Group(candidates.Cast<ITarget>().ToList(), minTargetCount, maxTargetCount, add);
    }

    protected IList<Targets> Group(IEnumerable<Effect> candidates, int minTargetCount, int? maxTargetCount = null, Action<ITarget, Targets> add = null)
    {
      return Group(candidates.Cast<ITarget>().ToList(), minTargetCount, maxTargetCount, add);
    }

    protected IList<Targets> Group(IEnumerable<Gameplay.Card.Card> candidates1, IEnumerable<Gameplay.Card.Card> candidates2,
      Action<ITarget, Targets> add1 = null, Action<ITarget, Targets> add2 = null)
    {
      return Group(candidates1.Cast<ITarget>().ToList(), candidates2.Cast<ITarget>().ToList(), add1, add2);
    }

    protected IList<Targets> Group(IList<ITarget> candidates1, IList<ITarget> candidates2,
      Action<ITarget, Targets> add1 = null, Action<ITarget, Targets> add2 = null)
    {
      var results = new List<Targets>();

      if (candidates1.Count == 0 || candidates2.Count == 0)
        return results;

      add1 = add1 ?? ((trg, trgts) => trgts.AddEffect(trg));
      add2 = add2 ?? ((trg, trgts) => trgts.AddEffect(trg));

      var index1 = 0;
      var index2 = 0;

      var groupCount = Math.Max(candidates1.Count, candidates2.Count);

      for (var i = 0; i < groupCount; i++)
      {
        // generate some options by mixing candidates 
        // from 2 selectors

        var targets = new Targets();

        add1(candidates1[index1], targets);
        add2(candidates2[index2], targets);

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

    protected IList<Targets> Group(IList<ITarget> candidates, List<int> damageDistribution)
    {
      var result = new Targets();

       foreach (var candidate in candidates)
      {
        result.AddEffect(candidate);
      }

      result.Distribution = damageDistribution;
      return new[] {result};
    }
    
    protected IList<Targets> Group(IList<ITarget> candidates, int minTargetCount, 
      int? maxTargetCount = null, Action<ITarget, Targets> add = null)
    {            
      var results = new List<Targets>();
      
      var targetCount = candidates.Count < maxTargetCount 
        ? minTargetCount 
        : maxTargetCount ?? minTargetCount;

      if (targetCount == 0)
        return results;

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

    protected int CalculateAttackerScore(Gameplay.Card.Card card)
    {
      return Combat.CouldBeBlockedByAny(card) ? 5 : 0 + card.CalculateCombatDamage(allDamageSteps: true);
    }

    protected static int CalculateAttackingPotencialScore(Gameplay.Card.Card card)
    {
      if (card.Has().DoesNotUntap || card.Has().CannotAttack)
        return 0;

      var damage = card.CalculateCombatDamage(allDamageSteps: true);

      if (card.Has().AnyEvadingAbility)
        return 2 + damage;

      return damage;
    }

    protected int CalculateBlockerScore(Gameplay.Card.Card card)
    {
      var count = Combat.CountHowManyThisCouldBlock(card);

      if (count > 0)
      {
        return count*10 + card.Toughness.GetValueOrDefault();
      }

      return 0;
    }

    protected IEnumerable<Gameplay.Card.Card> GetCandidatesForAttackerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetingRuleParameters p)
    {
      return p.Candidates<Gameplay.Card.Card>(ControlledBy.SpellOwner)
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

    protected IEnumerable<Gameplay.Card.Card> GetCandidatesForBlockerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetingRuleParameters p)
    {
      return p.Candidates<Gameplay.Card.Card>(ControlledBy.SpellOwner)
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

    protected IEnumerable<Gameplay.Card.Card> GetCandidatesThatCanBeDestroyed(TargetingRuleParameters p, Func<TargetsCandidates, IList<TargetCandidates>> selector = null)
    {
      return p.Candidates<Gameplay.Card.Card>(ControlledBy.SpellOwner, selector: selector)
        .Where(x => Stack.CanBeDestroyedByTopSpell(x.Card()) || Combat.CanBeDealtLeathalCombatDamage(x.Card()));
    }

    protected static IEnumerable<Gameplay.Card.Card> GetBounceCandidates(TargetingRuleParameters p, Func<TargetsCandidates, IList<TargetCandidates>> selector = null)
    {
      return p.Candidates<Gameplay.Card.Card>(ControlledBy.Opponent, selector: selector)
        .Select(x => new
          {
            Card = x,
            Score = x.Owner == p.Controller ? 2*x.Score : x.Score
          })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card);
    }
  }
}