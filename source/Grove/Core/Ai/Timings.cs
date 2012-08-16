namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Details.Mana;
  using Dsl;
  using Infrastructure;
  using Targeting;

  public delegate bool TimingDelegate(TimingParameters parameters);

  public static class Timings
  {
    public static TimingDelegate NoRestrictions()
    {
      return p => true;
    }

    public static TimingDelegate DeclareBlockers()
    {
      return Steps(Step.DeclareBlockers);
    }

    public static TimingDelegate CounterSpell(int? counterCost = null)
    {
      return p =>
        {
          if (p.Stack.TopSpell.Controller != p.Opponent)
            return false;

          return !counterCost.HasValue || !p.Opponent.HasMana(counterCost.Value, ManaUsage.Any);
        };
    }

    public static TimingDelegate ControllerHasConvertedMana(int converted)
    {
      return p => p.Controller.HasMana(converted, ManaUsage.Any);
    }

    public static TimingDelegate MassRemovalInstantSpeed()
    {
      return p =>
        {
          // play as response to some spells
          if (p.TopSpell != null && p.TopSpell.Controller == p.Opponent &&
            p.TopSpellCategoryIs(EffectCategories.Protector | EffectCategories.ToughnessIncrease))
          {
            return true;
          }

          // remove potential blockers
          if (p.Controller.IsActive && p.Step == Step.BeginningOfCombat)
          {
            return p.Opponent.Battlefield.CreaturesThatCanBlock.Count() > 0;
          }

          // damage or remove attackers
          if (!p.Controller.IsActive && p.Step == Step.DeclareAttackers)
          {
            return p.Combat.Attackers.Count() > 0;
          }

          // eot or when owner of ability is in trouble
          if ((!p.Controller.IsActive && p.Step == Step.EndOfTurn) || p.Stack.CanBeDestroyedByTopSpell(p.Card))
          {
            return true;
          }

          return false;
        };
    }

    public static TimingDelegate OnlyOneOfKind()
    {
      return p => p.Controller.Battlefield.None(x => x.Name == p.Card.Name);
    }

    public static TimingDelegate TargetRemovalInstant(bool combatOnly = false)
    {
      return p =>
        {
          // eot
          if (!p.Controller.IsActive && p.Step == Step.EndOfTurn && !combatOnly)
            return true;
                    
          // remove potential blockers
          if (p.Controller.IsActive && p.Target.IsCard())
          {
            return p.Step == Step.BeginningOfCombat && p.Target.Card().CanBlock();
          }

          // damage or remove attackers
          if (!p.Controller.IsActive && p.Target.IsCard())
          {
            return p.Step == Step.DeclareAttackers && p.Target.Card().IsAttacker;
          }

          if (combatOnly)
            return false;

          // play as response to some spells
          if (p.TopSpell != null && p.TopSpell.Controller == p.Opponent &&
            p.TopSpellCategoryIs(EffectCategories.Protector | EffectCategories.ToughnessIncrease))
          {
            if (p.TopSpell.AffectsSource)
            {
              return p.Target.Card() == p.TopSpell.Source.OwningCard;
            }

            if (p.TopSpell.HasTarget(p.Target.Card()))
              return true;
          }

          return false;
        };
    }

    public static TimingDelegate Lands()
    {
      return p => p.Step == Step.FirstMain;
    }

    public static TimingDelegate MainPhases()
    {
      return Steps(Step.FirstMain, Step.SecondMain);
    }

    public static TimingDelegate PassiveTurn()
    {
      return p => p.Opponent.IsActive;
    }

    public static TimingDelegate Regenerate()
    {
      return p =>
        {
          if (p.Card.Has().Indestructible)
            return false;

          if (p.Stack.CanBeDestroyedByTopSpell(p.Card))
            return true;

          return p.Combat.CanBeDealtLeathalCombatDamage(p.Card);
        };
    }

    public static TimingDelegate SacrificeCreatures(int count)
    {
      return p =>
        {
          var opponentCreatureCount = p.Opponent.Battlefield.Creatures.Count();

          if (opponentCreatureCount == 0)
            return false;

          if (opponentCreatureCount == 1)
            return p.Step == Step.FirstMain;

          if (opponentCreatureCount > 2*count + 1)
            return false;

          return p.Step == Step.SecondMain;
        };
    }

    public static TimingDelegate Turn(bool active = false, bool passive = false)
    {
      return p => (p.Controller.IsActive && active) || (!p.Controller.IsActive && passive);
    }

    public static TimingDelegate Steps(params Step[] steps)
    {
      return p => steps.Any(step => p.Step == step);
    }

    public static TimingDelegate NoRestrictions(params TimingDelegate[] predicates)
    {
      return p => predicates.All(predicate => predicate(p));
    }

    public static TimingDelegate Any(params TimingDelegate[] predicates)
    {
      return p => predicates.Any(predicate => predicate(p));
    }

    public static TimingDelegate AttachCombatEquipment()
    {
      return p =>
        {
          if (p.IsAttached)
          {
            // reattach to blocker
            return p.Step == Step.SecondMain;
          }

          // attach to attacker
          return p.Step == Step.FirstMain;
        };
    }

    public static TimingDelegate Leveler(IManaAmount activationCost, IEnumerable<LevelDefinition> levelDefinitions)
    {
      return p =>
        {
          var level = p.Card.Level ?? 0;
          int? costToNextLevel = null;

          foreach (var definition in levelDefinitions)
          {
            if (definition.Max == null)
              break;

            if (level < definition.Min)
            {
              costToNextLevel = definition.Min - level;
              break;
            }

            if (definition.Min <= level && definition.Max >= level)
            {
              costToNextLevel = definition.Max + 1 - level;
              break;
            }
          }

          if (costToNextLevel == null)
            return false;

          var manaCost = new AggregateManaAmount(Enumerable.Repeat(activationCost, costToNextLevel.Value));
          return p.Controller.HasMana(manaCost, ManaUsage.Abilities);
        };
    }

    public static TimingDelegate Cycling()
    {
      return p => p.Step == Step.EndOfTurn && !p.Controller.IsActive;
    }

    public static TimingDelegate FirstMain()
    {
      return Steps(Step.FirstMain);
    }

    public static TimingDelegate Creatures()
    {
      return p =>
        {
          if (p.Card.Power < 2 || p.Card.Has().Defender)
            return p.Step == Step.SecondMain;

          return p.Step == Step.FirstMain || p.Step == Step.SecondMain;
        };
    }

    public static TimingDelegate SecondMain()
    {
      return Steps(Step.SecondMain);
    }

    public static TimingDelegate InstantBounceAllCreatures()
    {
      return p =>
        {
          if (p.Step == Step.CombatDamage && p.Controller.IsActive)
          {
            var controllerCount = p.Controller.Battlefield.Creatures.Count(x => !x.IsTapped);
            var opponentCount = p.Opponent.Battlefield.Creatures.Count();

            return controllerCount < opponentCount;
          }

          return p.Step == Step.DeclareBlockers && !p.Controller.IsActive &&
            p.Attackers.Any();
        };
    }

    public static TimingDelegate DealsDamageWhenEntersBattlefield(int? amount = null)
    {
      return p =>
        {
          amount = amount ?? p.Activation.X;
          return p.Opponent.Battlefield.Creatures.Any(x => x.Life <= amount);
        };
    }

    public static TimingDelegate IncreaseOwnersPowerAndThougness(int? power, int? toughness)
    {
      return p =>
        {
          power = power ?? p.Activation.X;
          toughness = toughness ?? p.Activation.X;

          if (toughness > 0 && p.Stack.CanBeDealtLeathalDamageByTopSpell(p.Card))
          {
            return true;
          }

          if (p.Step == Step.DeclareBlockers && p.Controller.IsActive && p.Card.IsAttacker)
          {
            return QuickCombat.CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(
              attacker: p.Card,
              blockers: p.Combat.GetBlockers(p.Card),
              powerIncrease: power.Value,
              toughnessIncrease: toughness.Value) > 0;
          }

          if (p.Step == Step.DeclareBlockers && !p.Controller.IsActive && p.Card.IsBlocker)
          {
            return QuickCombat.CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(
              blocker: p.Card,
              attacker: p.Combat.GetAttacker(p.Card),
              powerIncrease: power.Value,
              toughnessIncrease: toughness.Value) > 0;
          }

          return false;
        };
    }

    public static TimingDelegate ChangeToCreature(int minAvailableMana)
    {            
      return p =>
        {
          if (p.Step == Step.BeginningOfCombat && p.Controller.IsActive)
          {
            return p.Controller.HasMana(minAvailableMana, ManaUsage.Abilities);
          }

          if (p.Step == Step.DeclareAttackers && !p.Controller.IsActive)
          {
            return p.Controller.HasMana(minAvailableMana, ManaUsage.Abilities) && p.Combat.Attackers.Any();
          }

          return false;
        };
    }

    public static TimingDelegate EndOfTurn()
    {
      return p =>
        {
          if (p.Step == Step.EndOfTurn)
            return true;

          return false;
        };
    }

    public static TimingDelegate BeforeDeath()
    {
      return p =>
        {
          if (p.Step == Step.DeclareBlockers)
          {
            return p.Combat.CanBeDealtLeathalCombatDamage(p.Card);
          }

          if (p.Stack.IsEmpty)
            return false;

          return p.Stack.CanBeDestroyedByTopSpell(p.Card) ||
            p.Stack.CanTopSpellReducePlayersLifeToZero(p.Controller);
        };
    }

    public static TimingDelegate DeclareAttackers()
    {
      return p => p.Step == Step.DeclareAttackers;
    }

    public static TimingDelegate EotWithAtLeast3CountersOrIfDestroyed()
    {
      return p =>
        {
          if (p.Card.Counters >= 3 && p.Step == Step.EndOfTurn && !p.Controller.IsActive)
            return true;

          if (p.Card.Counters >= 1 && p.Stack.CanBeDestroyedByTopSpell(p.Card, targetOnly: true))
            return true;

          return false;
        };
    }

    public static TimingDelegate SummonBlockers()
    {
      return p =>
        {
          if (p.Controller.IsActive)
            return false;

          if (p.Step != Step.DeclareAttackers)
            return false;

          return p.Combat.Attackers.Any();
        };
    }

    public static TimingDelegate OpponentControlsPermanent(Func<Card, bool> filter)
    {
      return p => p.Players.Permanents().Any(x => filter(x) && x.Controller == p.Opponent);
    }

    public static TimingDelegate ControllerHasAtLeastOneCardInGraveyard(Func<Card, bool> filter)
    {
      return p => p.Controller.Graveyard.Any(filter);
    }

    public static TimingDelegate IsCreature()
    {
      return p => p.Card.Is().Creature;
    }

    public static TimingDelegate HasCounters(int count)
    {
      return p => p.Card.Counters >= 3;
    }

    public static TimingDelegate HasCardInGraveyard(Func<Card,bool> predicate)
    {
      predicate = predicate ?? delegate { return true; };
      return p => p.Controller.Graveyard.Count(predicate) > 0;
    }

    public static TimingDelegate HasPermanent(Func<Card,bool> predicate)
    {
      predicate = predicate ?? delegate { return true; };
      return p => p.Controller.Battlefield.Count(predicate) > 0;
    }
  }
}