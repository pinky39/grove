namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Details.Mana;
  using Infrastructure;
  using Targeting;

  public static class TargetSelectorAi
  {
    public delegate IEnumerable<ITarget> InputSelectorDelegate(TargetsCandidates candidates);

    public delegate IEnumerable<Targets> OutputSelectorDelegate(IEnumerable<ITarget> targets);

    public static TargetSelectorAiDelegate OrderByDescendingScore(Controller controller = Ai.Controller.Opponent)
    {
      return p =>
        {
          var candidates = p.Candidates();

          switch (controller)
          {
            case Ai.Controller.Opponent:
              candidates = candidates
                .RestrictController(p.Opponent);
              break;
            case Ai.Controller.SpellOwner:
              candidates = candidates
                .RestrictController(p.Controller);
              break;
          }

          var orderedCandidates = candidates
            .OrderByDescending(x => x.Card().Score)
            .Select(x => x.Card())
            .ToList();

          var targetCount = p.Selector.GetEffectTargetCount();

          if (p.IsTriggeredAbilityTarget && targetCount == 1 && orderedCandidates.Count == 0)
          {
            return p.Targets(p.Candidates().OrderBy(x => x.Card().Score));
          }

          if (orderedCandidates.Count < targetCount)
            return p.NoTargets();
          
          if (targetCount == 1)
          {
            return p.Targets(candidates);
          }          
          
          var grouped = GroupCandidates(orderedCandidates, targetCount);          
          return p.MultipleTargets(grouped);
        };
    }

    public static TargetSelectorAiDelegate TapOpponentsCreatures()
    {
      return p =>
        {
          if (p.Step == Step.BeginningOfCombat && p.Controller.IsActive)
          {
            return p.Targets(p.Opponent.ToEnumerable());
          }

          return p.NoTargets();
        };
    }

    public static TargetSelectorAiDelegate TapOpponentsLands()
    {
      return p =>
        {
          if (p.Step == Step.Upkeep && !p.Controller.IsActive)
          {
            return p.Targets(p.Opponent.ToEnumerable());
          }

          return p.NoTargets();
        };
    }

    public static TargetSelectorAiDelegate UntapYourCreatures()
    {
      return p =>
        {
          if (p.Step == Step.DeclareAttackers && !p.Controller.IsActive)
          {
            return p.Targets(p.Controller.ToEnumerable());
          }

          return p.NoTargets();
        };
    }


    public static TargetSelectorAiDelegate Opponent()
    {
      return p =>
        {
          return p.Targets(p.Candidates()
            .Where(x => x.Player() == p.Opponent));
        };
    }

    public static TargetSelectorAiDelegate PumpAttackerOrBlocker(int? power, int? thougness)
    {
      return p =>
        {
          power = power ?? p.MaxX;
          thougness = thougness ?? p.MaxX;


          if (p.Controller.IsActive && p.Step == Step.DeclareBlockers)
          {
            var candidates = GetCandidatesForAttackerPowerToughnessIncrease(power, thougness, p);
            return p.Targets(candidates);
          }

          if (!p.Controller.IsActive && p.Step == Step.DeclareBlockers)
          {
            var candidates = GetCandidatesForBlockerPowerToughnessIncrease(power, thougness, p);
            return p.Targets(candidates);
          }

          return p.NoTargets();
        };
    }

    private static IEnumerable<Card> GetCandidatesForBlockerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetSelectorAiParameters p)
    {
      return p.Candidates().RestrictController(p.Controller)
        .Where(x => x.Card().IsBlocker)
        .Select(
          x =>
            new
              {
                Card = x.Card(),
                Gain =
                  QuickCombat.CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(
                    blocker: x.Card(),
                    attacker: p.Combat.GetAttacker(x.Card()),
                    powerIncrease: powerIncrease.Value,
                    toughnessIncrease: toughnessIncrease.Value)
              })
        .Where(x => x.Gain > 0)
        .OrderByDescending(x => x.Gain)
        .Select(x => x.Card);
    }


    private static IEnumerable<Card> GetCandidatesForAttackerPowerToughnessIncrease(int? powerIncrease,
      int? toughnessIncrease, TargetSelectorAiParameters p)
    {
      return p.Candidates().RestrictController(p.Controller)
        .Where(x => x.Card().IsAttacker)
        .Select(
          x =>
            new
              {
                Card = x.Card(),
                Gain =
                  QuickCombat.CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(
                    attacker: x.Card(),
                    blockers: p.Combat.GetBlockers(x.Card()),
                    powerIncrease: powerIncrease.Value,
                    toughnessIncrease: toughnessIncrease.Value)
              })
        .Where(x => x.Gain > 0)
        .OrderByDescending(x => x.Gain)
        .Select(x => x.Card);
    }

    public static TargetSelectorAiDelegate CounterSpell()
    {
      return p =>
        {
          var candidates = p.Candidates().RestrictController(p.Opponent)
            .Take(1);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate CombatEquipment()
    {
      return p =>
        {
          if (p.Step == Step.FirstMain)
          {
            return p.Targets(p.Candidates()
              .Where(x => x.Card().CanAttack)
              .Select(x => new
                {
                  Card = x.Card(),
                  Score = CalculateAttackerScore(x.Card(), p.Combat)
                })
              .OrderByDescending(x => x.Score)
              .Where(x => x.Score > 0)
              .Select(x => x.Card));
          }

          return p.Targets(p.Candidates()
            .Where(x => x.Card().CanBlock())
            .Select(x => new
              {
                Card = x.Card(),
                Score = CalculateBlockerScore(x.Card(), p.Combat)
              })
            .OrderByDescending(x => x.Score)
            .Where(x => x.Score > 0)
            .Select(x => x.Card));
        };
    }

    public static TargetSelectorAiDelegate DealDamageSingleSelector(Func<TargetSelectorAiParameters, int> amount)
    {
      return p => DealDamageSingleSelector(amount(p))(p);
    }

    public static TargetSelectorAiDelegate DealDamageMultipleSelectors(int amount)
    {
      return p =>
        {
          // multiple selectors, vary targets only from first
          // since the combinatorial complexity would be too great
          // otherwise

          var all = new List<IList<ITarget>>();

          var firstSelectorCandidates = p.EffectCandidates[0]
            .OrderByDamageScore(amount, p)
            .ToList();

          if (firstSelectorCandidates.Count == 0)
            return p.NoTargets();

          all.Add(firstSelectorCandidates);

          for (var i = 1; i < p.EffectCandidates.Count; i++)
          {
            var otherSelectorCandidate = p.EffectCandidates[i]
              .OrderByDamageScore(amount, p)
              .FirstOrDefault();


            if (otherSelectorCandidate == null)
              return p.NoTargets();

            // replicate the candidate for each target in
            // first selector

            all.Add(
              Enumerable.Repeat(
                otherSelectorCandidate,
                firstSelectorCandidates.Count).ToList());
          }

          return p.MultipleTargets(all);
        };
    }

    private static IEnumerable<ITarget> OrderByDamageScore(this IEnumerable<ITarget> targets, int amount,
      TargetSelectorAiParameters p)
    {
      var candidates = targets
        .Where(x => x == p.Opponent)
        .Select(x => new
          {
            Target = x,
            Score = ScoreCalculator.CalculateLifelossScore(x.Player().Life, amount)
          })
        .Concat(
          targets
            .Where(x => x.IsCard() && x.Card().Controller == p.Opponent)
            .Select(x => new
              {
                Target = x,
                Score = x.Card().Life <= amount ? x.Card().Score : 0
              }))
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Select(x => x.Target);

      return candidates;
    }

    public static TargetSelectorAiDelegate DealDamageSingleSelector(int? amount = null)
    {
      return p =>
        {
          amount = amount ?? p.MaxX;

          var candidates = p.Candidates()
            .OrderByDamageScore(amount.Value, p)
            .ToList();

          var targetCount = p.Selector.GetEffectTargetCount();

          if (candidates.Count < targetCount)
          {
            return p.NoTargets();
          }

          if (targetCount == 1)
          {
            var targets = p.Targets(candidates);

            // triggered abilities force you to choose a target even if its
            // not favorable e.g Flaming Kavu
            if (p.IsTriggeredAbilityTarget && targets.Count == 0)
            {
              targets = p.Targets(p.Candidates().OrderByDescending(x => x.Card().Toughness));
            }

            return targets;
          }

          // if multiple targets vary only the last target          
          var targetsCandidates = new List<IList<ITarget>>();
          for (var i = 0; i < candidates.Count - 1; i++)
          {
            var targetCandidates = Enumerable
              .Repeat(candidates[i], candidates.Count - targetCount + 1)
              .ToList();

            targetsCandidates.Add(targetCandidates);
          }

          targetsCandidates.Add(candidates.Skip(candidates.Count - 1)
            .ToList());


          return p.MultipleTargets(targetsCandidates);
        };
    }

    private static int CalculateAttackerScore(Card card, Combat combat)
    {
      return combat.CouldBeBlockedByAny(card) ? 5 : 0 + card.CalculateCombatDamage(allDamageSteps: true);
    }

    private static int CalculateBlockerScore(Card card, Combat combat)
    {
      var count = combat.CountHowManyThisCouldBlock(card);

      if (count > 0)
      {
        return count*10 + card.Toughness.Value;
      }

      return 0;
    }

    public static TargetSelectorAiDelegate CombatEnchantment()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .Where(x => x.Card().Controller == p.Controller)
            .Where(x => x.Card().CanAttack)
            .Select(x => new
              {
                Card = x.Card(),
                Score = CalculateAttackerScore(x.Card(), p.Combat)
              })
            .OrderByDescending(x => x.Score)
            .Where(x => x.Score > 0)
            .Select(x => x.Card);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate ShieldHexproof()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .Where(x => x.Card().Controller == p.Controller)
            .Where(x => p.Stack.CanBeDestroyedByTopSpell(x.Card(), targetOnly: true))
            .OrderByDescending(x => x.Card().Score);

          return p.Targets(candidates);
        };
    }

    private static IEnumerable<ITarget> GetPermanentsThatCanBeKilledOrderedByScore(this IEnumerable<ITarget> targets,
      TargetSelectorAiParameters p)
    {
      return targets
        .Where(x => x.Card().Controller == p.Controller)
        .Where(x => p.Stack.CanBeDestroyedByTopSpell(x.Card()) || p.Combat.CanBeDealtLeathalCombatDamage(x.Card()))
        .OrderByDescending(x => x.Card().Score);
    }

    public static TargetSelectorAiDelegate ShieldIndestructible()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .GetPermanentsThatCanBeKilledOrderedByScore(p);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate Destroy()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .Where(x => x.Card().Controller == p.Opponent)
            .OrderByDescending(x => x.Card().Score);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate Any(params TargetSelectorAiDelegate[] delegates)
    {
      return p =>
        {
          var result = new List<Targets>();
          result.AddRange(delegates[0](p));

          for (var i = 1; i < delegates.Length; i++)
          {
            var filterDelegate = delegates[i];
            var targetsList = filterDelegate(p);
            foreach (var targets in targetsList)
            {
              if (result.Contains(targets))
                continue;

              result.Add(targets);
            }
          }

          return result;
        };
    }

    public static TargetSelectorAiDelegate ReduceToughness(int? amount = null)
    {
      return p =>
        {
          amount = amount ?? p.MaxX;

          var candidates = p.Candidates()
            .Where(x => x.Card().Controller == p.Opponent)
            .Select(x => new
              {
                Target = x,
                Score = x.Card().Life <= amount ? x.Card().Score : 0
              })
            .OrderByDescending(x => x.Score)
            .Select(x => x.Target);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate IncreasePowerAndToughness(int? power, int? toughness)
    {
      return p =>
        {
          IList<Card> candidates;

          if (p.Controller.IsActive && p.Step == Step.DeclareBlockers)
          {
            candidates = GetCandidatesForAttackerPowerToughnessIncrease(power, toughness, p)
              .ToList();
          }

          else if (!p.Controller.IsActive && p.Step == Step.DeclareBlockers)
          {
            candidates = GetCandidatesForBlockerPowerToughnessIncrease(power, toughness, p).ToList();
          }

          else if ((!p.Controller.IsActive && p.Step == Step.EndOfTurn) ||
            (p.Source.IsPermanent && p.Stack.CanBeDestroyedByTopSpell(p.Source)))
          {
            candidates = p.Candidates()
              .Where(x => x.Card().Controller == p.Controller)
              .OrderByDescending(x => x.Card().Score)
              .Select(x => x.Card()).ToList();
          }
          else
          {
            candidates = p.Candidates()
              .Where(x => x.Card().Controller == p.Controller)
              .Where(x => p.Stack.CanBeDealtLeathalDamageByTopSpell(x.Card()))
              .Select(x => x.Card())
              .ToList();
          }

          var targetCount = p.Selector.GetEffectTargetCount();

          if (candidates.Count < targetCount)
            return p.NoTargets();

          if (targetCount == 1)
          {
            return p.Targets(candidates);
          }

          var targetsCandidates = GroupCandidates(candidates, targetCount);
          return p.MultipleTargets(targetsCandidates);
        };
    }

    private static List<IList<Card>> GroupCandidates(IList<Card> candidates, int targetCount)
    {
      var targetsCandidates = new List<IList<Card>>();

      for (var i = 0; i < candidates.Count - 1; i++)
      {
        var targetCandidates = Enumerable
          .Repeat(candidates[i], candidates.Count - targetCount + 1)
          .ToList();

        targetsCandidates.Add(targetCandidates);
      }

      targetsCandidates.Add(candidates.Skip(candidates.Count - 1)
        .ToList());
      return targetsCandidates;
    }

    public static TargetSelectorAiDelegate CostTapOrSacCreature(bool canUseSelf = true)
    {
      return p =>
        {
          var candidates = p.Candidates()
            .Where(x => canUseSelf || x != p.Source)
            .OrderBy(x => x.Card().Score);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate CostSacrificeGainLife()
    {
      return p =>
        {
          if (p.Stack.CanTopSpellReducePlayersLifeToZero(p.Controller))
          {
            return p.Targets(p.Candidates()
              .OrderBy(x => x.Card().Score));
          }

          var candidates = new List<ITarget>();

          if (p.Step == Step.DeclareBlockers)
          {
            candidates.AddRange(
              p.Candidates()
                .Where(x => p.Combat.CanBeDealtLeathalCombatDamage(x.Card()))
                .Where(x => !p.Combat.CanKillAny(x.Card())));
          }

          candidates.AddRange(
            p.Candidates()
              .Where(x => p.Stack.CanBeDestroyedByTopSpell(x.Card())));

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate Bounce()
    {
      return Destroy();
    }

    public static TargetSelectorAiDelegate Controller()
    {
      return p =>
        {
          return p.Targets(p.Candidates()
            .Where(x => x.Player() == p.Controller));
        };
    }

    public static TargetSelectorAiDelegate Exile()
    {
      return Destroy();
    }

    public static TargetSelectorAiDelegate TapCreature()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .Where(x => x.Card().Controller == p.Opponent)
            .Where(x => !x.Card().IsTapped)
            .OrderByDescending(x => x.Card().CalculateCombatDamage(allDamageSteps: true));

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate CreatureWithGreatestPower()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .OrderByDescending(x => x.Card().Power)
            .Take(1);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate PreventAllDamageFromSourceToTarget()
    {
      return p =>
        {
          var targetPicks = new List<ITarget>();
          var sourcePicks = new List<ITarget>();

          if (!p.Stack.IsEmpty)
          {
            var damageToPlayer = p.Stack.GetDamageTopSpellWillDealToPlayer(p.Controller);

            if (damageToPlayer > 0)
            {
              targetPicks.Add(p.Controller);
              sourcePicks.Add(p.Stack.TopSpell);
            }

            var creatureKilledByTopSpell = p.EffectCandidates[1]
              .Where(x => x.IsCard())
              .Where(x => p.Stack.CanBeDealtLeathalDamageByTopSpell(x.Card()))
              .OrderByDescending(x => x.Card().Score)
              .FirstOrDefault();

            if (creatureKilledByTopSpell != null)
            {
              targetPicks.Add(creatureKilledByTopSpell);
              sourcePicks.Add(p.Stack.TopSpell);
            }
          }

          if (p.Step == Step.DeclareBlockers)
          {
            if (p.Controller.IsActive == false)
            {
              var attacker = p.Combat.GetAttackerWhichWillDealGreatestDamageToDefender();

              if (attacker != null)
              {
                targetPicks.Add(p.Controller);
                sourcePicks.Add(attacker);
              }

              var blockerAttackerPair = p.EffectCandidates[1]
                .Where(x => x.IsCard() && x.Card().IsBlocker)
                .Select(x => new
                  {
                    Target = x.Card(),
                    Source =
                      QuickCombat.GetAttackerThatDealsLeathalDamageToBlocker(x.Card(), p.Combat.GetAttacker(x.Card()))
                  })
                .Where(x => x.Source != null)
                .OrderByDescending(x => x.Target.Score)
                .FirstOrDefault();

              if (blockerAttackerPair != null)
              {
                targetPicks.Add(blockerAttackerPair.Target);
                sourcePicks.Add(blockerAttackerPair.Source);
              }
            }
            else
            {
              var blockerAttackerPair = p.EffectCandidates[1]
                .Where(x => x.IsCard() && x.Card().IsAttacker)
                .Select(x => new
                  {
                    Target = x.Card(),
                    Source =
                      QuickCombat.GetBlockerThatDealsLeathalDamageToAttacker(x.Card(), p.Combat.GetBlockers(x.Card()))
                  })
                .Where(x => x.Source != null)
                .OrderByDescending(x => x.Target.Score)
                .FirstOrDefault();

              if (blockerAttackerPair != null)
              {
                targetPicks.Add(blockerAttackerPair.Target);
                sourcePicks.Add(blockerAttackerPair.Source);
              }
            }
          }

          return p.MultipleTargets(sourcePicks, targetPicks);
        };
    }

    public static TargetSelectorAiDelegate DamageRedirection()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .Select(x => new
              {
                Card = x.Card(),
                Score = CalculateDamageRedirectionScore(x.Card(), p)
              })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Card);

          return p.Targets(candidates);
        };
    }

    private static int CalculateDamageRedirectionScore(Card card, TargetSelectorAiParameters p)
    {
      const int protectionOffset = 200;

      if (card.Controller == p.Opponent)
      {
        return card.Score;
      }

      if (card.HasProtectionFrom(ManaColors.Red | ManaColors.Black))
      {
        return protectionOffset + card.Score;
      }

      return card.Toughness.Value - 3;
    }

    public static TargetSelectorAiDelegate PreventDamageFromSourceToController()
    {
      return p =>
        {
          var targetPicks = new List<ITarget>();

          if (!p.Stack.IsEmpty && p.Candidates().Contains(p.Stack.TopSpell))
          {
            var damageToPlayer = p.Stack.GetDamageTopSpellWillDealToPlayer(p.Controller);

            if (damageToPlayer > 0)
            {
              targetPicks.Add(p.Stack.TopSpell);
            }
          }

          if (p.Step == Step.DeclareBlockers)
          {
            if (!p.Controller.IsActive)
            {
              var attacker = p.Combat.GetAttackerWhichWillDealGreatestDamageToDefender(
                card => p.Candidates().Contains(card));

              if (attacker != null)
              {
                targetPicks.Add(attacker);
              }
            }
          }

          return p.Targets(targetPicks);
        };
    }

    public static TargetSelectorAiDelegate GreatestConvertedManaCost()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .OrderBy(x => x.Card().ManaCost.Converted)
            .Take(1);

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate PreventNextDamageToCreatureOrPlayer(int amount)
    {
      return p =>
        {
          if (!p.Stack.IsEmpty)
          {
            var playerCandidate = p.Candidates()
              .Where(x => x == p.Controller)
              .Where(x => p.Stack.GetDamageTopSpellWillDealToPlayer(x.Player()) > 0);

            var cardCandidates = p.Candidates()
              .Where(x => x.IsCard() && x.Card().Controller == p.Controller)
              .Select(x => x.Card())
              .Where(x =>
                {
                  var damageToCreature = p.Stack.GetDamageTopSpellWillDealToCreature(x);
                  return (damageToCreature >= x.Life) &&
                    (damageToCreature - amount < x.Life);
                })
              .OrderByDescending(x => x.Score);

            return p.Targets(playerCandidate.Concat(cardCandidates));
          }

          if (p.Step == Step.DeclareBlockers)
          {
            if (p.Controller.IsActive)
            {
              var cardCandidates = p.Candidates()
                .Where(x => x.IsCard() && x.Card().Controller == p.Controller && x.Card().IsAttacker)
                .Select(x => x.Card())
                .Where(x =>
                  {
                    var prevented = QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(
                      attacker: x.Card(),
                      blockers: p.Combat.GetBlockers(x.Card()));

                    return 0 < prevented && prevented <= amount;
                  })
                .OrderByDescending(x => x.Score);

              return p.Targets(cardCandidates);
            }
            else
            {
              var playerCandidate = p.Candidates()
                .Where(x => x == p.Controller)
                .Where(x => p.Combat.WillAnyAttackerDealDamageToDefender());

              var cardCandidates = p.Candidates()
                .Where(x => x.IsCard() && x.Card().Controller == p.Controller && x.Card().IsBlocker)
                .Select(x => x.Card())
                .Where(x =>
                  {
                    var prevented = QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeBlockerFromDying(
                      blocker: x.Card(),
                      attacker: p.Combat.GetAttacker(x.Card()));


                    return 0 < prevented && prevented <= amount;
                  })
                .OrderByDescending(x => x.Score);

              return p.Targets(playerCandidate.Concat(cardCandidates));
            }
          }

          return p.NoTargets();
        };
    }

    public static TargetSelectorAiDelegate PreventAttackerDamage()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .OrderByDescending(x => x.Card().CalculateCombatDamage(allDamageSteps: true));

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate Pacifism()
    {
      return p =>
        {
          var candidates = p.Candidates()
            .Where(x => x.Card().Controller == p.Opponent)
            .Select(x => x.Card())
            .Select(x => new
              {
                Card = x,
                Score = CalculateAttackingScore(x)
              })
            .OrderByDescending(x => x.Score)
            .Select(x => x.Card)
            .ToList();

          if (p.IsTriggeredAbilityTarget && candidates.Count == 0)
          {
            return p.Targets(p.Candidates().OrderBy(x => x.Card().Power));
          }

          return p.Targets(candidates);
        };
    }

    public static TargetSelectorAiDelegate DealDamageSingleSelectorDistribute(int? amount = null)
    {
      return p =>
        {
          amount = amount ?? p.MaxX;

          // this is a 0-1 knapsack optimization problem

          var targets = p.Candidates()
            .Where(x => x.Is().Creature && x.Life() <= amount)
            .Select(x => new KnapsackItem<ITarget>(
              item: x,
              weight: x.Life(),
              value: x.Card().Score))
            .ToList();

          targets.AddRange(p.Candidates()
            .Where(x => x == p.Opponent)
            .SelectMany(x =>
              {
                var items = new List<KnapsackItem<ITarget>>();

                const int decrementSoPlayerAtMostOnce = 1;

                for (var i = 1; i <= amount.Value; i++)
                {
                  items.Add(
                    new KnapsackItem<ITarget>(
                      item: x,
                      weight: i,
                      value: ScoreCalculator.CalculateLifelossScore(x.Player().Life, i) - decrementSoPlayerAtMostOnce));
                }

                return items;
              })
            );

          if (targets.Count == 0)
            return p.NoTargets();

          var solution = Knapsack.Solve(targets, amount.Value);
          var selected = solution.Select(x => x.Item).ToList();

          var distribution = new DamageDistributor
            {
              Distribution = (t, a) => solution
                .Select(x => x.Weight)
                .ToList()
            };

          return p.MultipleTargets(distribution, selected);
        };
    }

    public static TargetSelectorAiDelegate BounceSelfAndTargetCreatureYouControl()
    {
      return p =>
        {
          var targetCandidates = p.Candidates()
            .GetPermanentsThatCanBeKilledOrderedByScore(p)
            .ToList();

          if (targetCandidates.Count == 1 && targetCandidates[0] == p.Source)
          {
            // if owner is the only one that can be killed
            // target the owner
            return p.Targets(targetCandidates);
          }

          // otherwise return all except the owner
          return p.Targets(targetCandidates.Where(x => x != p.Source));
        };
    }

    public static TargetSelectorAiDelegate GainControl()
    {
      return Destroy();
    }

    public static TargetSelectorAiDelegate ReducePower(int? amount)
    {
      return p =>
        {
          amount = amount ?? p.MaxX;

          var candidates = p.Candidates()
            .Where(x => x.Card().Controller == p.Opponent)
            .Select(x => new
              {
                Card = x.Card(),
                Score = CalculateAttackingScore(x.Card())
              })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Card);


          return p.Targets(candidates);
        };
    }

    private static int CalculateAttackingScore(Card card)
    {
      if (card.Has().DoesNotUntap || card.Has().CannotAttack)
        return 0;

      var damage = card.CalculateCombatDamage(allDamageSteps: true);

      if (card.Has().AnyEvadingAbility)
        return 2 + damage;

      return damage;
    }
  }
}