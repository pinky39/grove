namespace Grove.Core.Ai
{
  using System;
  using System.Linq;

  public static class Timings
  {
    public static bool Combat(Game game, Card card, ActivationParameters spell)
    {
      return Steps(Step.DeclareBlockers)(game, card, spell);
    }

    public static Func<Game, Card, ActivationParameters, bool> ControllerHasMana(ManaAmount manaAmount)
    {
      return (game, card, activationParameters) => card.Controller.HasEnoughMana(manaAmount);
    }

    public static Func<Game, Card, ActivationParameters, bool> CounterSpell(int? doNotCounterCost = null)
    {
      return (game, card, activationParameters) => {
        if (!doNotCounterCost.HasValue)
          return true;

        return !game.Players.GetOpponent(card.Controller).HasEnoughMana(doNotCounterCost.Value);
      };
    }

    public static bool InstantRemoval(Game game, Card card, ActivationParameters activationParameters)
    {
      return Steps(Step.BeginningOfCombat, Step.DeclareBlockers)(game, card, activationParameters);
    }

    public static bool Lands(Game game, Card card, ActivationParameters spell)
    {
      return game.Turn.Step == Step.FirstMain;
    }

    public static bool MainPhases(Game game, Card card, ActivationParameters activationParameters)
    {
      return Steps(Step.FirstMain, Step.SecondMain)(game, card, activationParameters);
    }

    public static bool OnlyDuringOpponentTurn(Game game, Card card, ActivationParameters activationParameters)
    {
      var opponent = game.Players.GetOpponent(card.Controller);
      return opponent.IsActive;
    }

    public static bool Regenerate(Game game, Card card, ActivationParameters spell)
    {
      return ResponseToSpell(EffectCategories.Destruction | EffectCategories.DamageDealing)(game, card, spell) ||
        Steps(Step.DeclareBlockers)(game, card, spell);
    }

    public static Func<Game, Card, ActivationParameters, bool> ResponseToSpell(EffectCategories effectCategories = EffectCategories.Generic)
    {
      return (game, card, activationParameters) => {
        var topSpell = game.Stack.TopSpell;

        if (topSpell == null)
          return false;

        if (topSpell.HasTarget && (
          topSpell.Target != activationParameters.EffectTarget &&
            topSpell.Target != activationParameters.CostTarget &&
              topSpell.Target != card &&
                topSpell.Target != card.Controller))
        {
          return false;
        }

        if (effectCategories == EffectCategories.Generic)
          return true;

        return ((effectCategories & topSpell.Source.EffectCategories) != EffectCategories.Generic);
      };
    }

    public static Func<Game, Card, ActivationParameters, bool> SacrificeCreatures(int count)
    {
      return (game, card, activationParameters) => {
        var opponent = game.Players.WithoutPriority;

        if (opponent.Battlefield.Creatures.Count() == 0)
          return false;

        if (opponent.Battlefield.Creatures.Count() == 1)
          return game.Turn.Step == Step.FirstMain;

        return game.Turn.Step == Step.SecondMain;
      };
    }

    public static Func<Game, Card, ActivationParameters, bool> Steps(params Step[] steps)
    {
      return (game, card, activationParameters) => steps.Any(step => game.Turn.Step == step);
    }

    public static bool WillBeDealtLeathalCombatDamage(Game game, Card card, ActivationParameters spell)
    {
      if (!Steps(Step.DeclareBlockers)(game, card, spell))
      {
        return false;
      }

      return game.Combat.IsCannonfodder(card);
    }
  }
}