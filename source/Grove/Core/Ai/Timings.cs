namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using CardDsl;

  public delegate bool TimingDelegate(TimingParameters parameters);

  public static class Timings
  {
    public static TimingDelegate Combat()
    {
      return Steps(Step.DeclareBlockers);
    }

    public static TimingDelegate CounterSpell(int? counterCost = null)
    {
      return p =>
        {
          if (p.TopSpellController != p.Opponent)
            return false;

          return !counterCost.HasValue || !p.Opponent.HasMana(counterCost.Value);
        };
    }

    public static TimingDelegate AtLeastOneAttacker(
      int maxPower = int.MaxValue, int maxToughness = int.MaxValue)
    {
      return p =>
        {
          Func<Attacker, bool> predicate = (attacker) =>
            attacker.Card.Power <= maxPower &&
              attacker.Card.Toughness <= maxToughness;

          return p.Attackers.Where(predicate).Count() > 1;
        };
    }

    public static TimingDelegate ControllerHasConvertedMana(int converted)
    {
      return p => p.Controller.HasMana(converted);
    }

    public static TimingDelegate InstantRemoval()
    {
      return p =>
        {
          // non target instant removal
          if (p.Target == null)
          {
            return p.Opponent.IsActive && p.Step == Step.EndOfTurn;
          }

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

          // target player
          return !p.Controller.IsActive && p.Step == Step.EndOfTurn && p.Target.IsPlayer();
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

    public static TimingDelegate OnlyDuringOpponentTurn()
    {
      return p => p.Opponent.IsActive;
    }

    public static TimingDelegate Regenerate()
    {
      return p =>
        {
          if (p.Card.Has().Indestructible)
            return false;

          if (p.CanThisBeDestroyedByTopSpell())
            return true;

          if (p.Step == Step.DeclareBlockers && p.CanThisBeDealtLeathalCombatDamage())
          {
            return true;
          }

          return false;
        };
    }

    public static TimingDelegate ResponseToSpell(EffectCategories effectCategories = EffectCategories.Generic)
    {
      return p =>
        {
          if (p.TopSpell == null)
            return false;

          if (p.TopSpell.HasTarget && !p.IsTopSpellTarget)
          {
            return false;
          }

          if (effectCategories == EffectCategories.Generic)
            return true;

          return p.TopSpell.HasCategory(effectCategories);
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

    public static TimingDelegate IsCannonFodder()
    {
      return All(
        Steps(Step.DeclareBlockers),
        p => p.IsCannonfodder());
    }

    public static TimingDelegate All(params TimingDelegate[] predicates)
    {
      return p => predicates.All(predicate => predicate(p));
    }

    public static TimingDelegate Any(params TimingDelegate[] predicates)
    {
      return p => predicates.Any(predicate => predicate(p));
    }

    public static TimingDelegate AttachEquipment()
    {
      return p =>
        {
          if (p.IsAttached)
          {
            // reattach to blocker
            return p.Step == Step.SecondMain && p.Target.Card().CanBlock();
          }

          // attach to attacker
          return p.Step == Step.FirstMain && p.Target.Card().CanAttack;
        };
    }

    public static TimingDelegate Leveler(IManaAmount activationCost, IEnumerable<LevelDefinition> levelDefinitions)
    {
      return p =>
        {
          if (p.Step != Step.FirstMain)
            return false;

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
          return p.Controller.HasMana(manaCost);
        };
    }
  }
}