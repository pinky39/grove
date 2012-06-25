namespace Grove.Core.Ai
{
  using System;
  using System.Linq;

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
      return
        Any(
          All(
            Turn(active: true),
            Steps(Step.BeginningOfCombat)
            ),
          All(
            Turn(passive: true),
            Steps(Step.DeclareAttackers))
          );
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

    //public static Func<Game, Card, ActivationParameters, bool> RegenerateBetter()
    //{
    //  return (game, card, activationParameters) =>
    //    {


    //    };
    //}

    public static TimingDelegate Regenerate()
    {
      return
        Any(
          ResponseToSpell(EffectCategories.Destruction | EffectCategories.DamageDealing),
          Steps(Step.DeclareBlockers)
          );
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
  }
}