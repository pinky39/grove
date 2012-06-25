namespace Grove.Core.Ai
{
  using System;
  using System.Linq;

  public static class TargetScores
  {
    public static Core.ScoreCalculator LessValuableCardsScoreMore()
    {
      return (target, source, maxX, game) =>
        {
          if (target.Card().Is().Token)
            return Convert.ToInt32(WellKnownTargetScores.Good - (target.Card().Power + target.Card().Toughness));

          return WellKnownTargetScores.Good - target.Card().ManaCost.Count();
        };
    }

    public static Core.ScoreCalculator OpponentOnly()
    {
      return (target, source, maxX, game) =>
        {
          var opponent = game.Players.GetOpponent(source.Controller);
          
          return target == opponent 
            ? WellKnownTargetScores.Good 
            : WellKnownTargetScores.NotAccepted;
        };
    }
    
    public static Core.ScoreCalculator OpponentStuffScoresMore(
      int? spellsDamage = null, bool considerSpellController = false, bool reducesPwt = false)
    {
      return (target, source, maxX, game) =>
        {
          var opponent = game.Players.GetOpponent(source.Controller);

          if (target.IsCard())
          {
            var controller = target.Card().Controller;

            if (controller != opponent)
            {
              return considerSpellController ? WellKnownTargetScores.Bad : WellKnownTargetScores.NotAccepted;
            }

            if (target.Card().Has().Indestructible && spellsDamage > 0 && !reducesPwt)
            {
              return WellKnownTargetScores.NotAccepted;
            }

            var score = target.Card().Score;

            if (spellsDamage > 0 && target.Card().LifepointsLeft > spellsDamage)
            {
              score = score/2;
            }

            return WellKnownTargetScores.Good + score;
          }

          if (target.IsEffect())
          {
            if (target.Effect().Controller != game.Players.GetOpponent(source.Controller))
              return considerSpellController
                ? WellKnownTargetScores.Bad
                : WellKnownTargetScores.NotAccepted;

            return WellKnownTargetScores.Good;
          }

          if (target.Player() == opponent)
          {
            var rank = WellKnownTargetScores.Good;

            if (spellsDamage > 0 || maxX > 0)
            {
              var damage = spellsDamage ?? maxX;
              rank += ScoreCalculator.CalculateLifelossScore(opponent.Life, damage.Value);
            }

            return rank;
          }

          return WellKnownTargetScores.NotAccepted;
        };
    }

    public static Core.ScoreCalculator YourStuffScoresMore()
    {
      return (target, source, maxX, game) =>
        {
          var opponent = game.Players.GetOpponent(source.Controller);

          if (target.IsCard())
          {
            return target.Card().Controller == opponent
              ? WellKnownTargetScores.NotAccepted
              : WellKnownTargetScores.Good;
          }

          return target.Player() != opponent
            ? WellKnownTargetScores.Good
            : WellKnownTargetScores.NotAccepted;
        };
    }

    public static Core.ScoreCalculator YourCreatureWithGreatestPowerOnly()
    {
      return (target, source, maxX, game) =>
        {
          var greatestPower = source.Controller.Battlefield.Creatures
            .Where(x => x.CanBeTargetBySpellsOwnedBy(source.Controller))
            .Max(x => x.Power);
          
          return target.Card().Power == greatestPower
            ? WellKnownTargetScores.Good
            : WellKnownTargetScores.NotAccepted;
        };
    }

    public static Core.ScoreCalculator BattlefieldRanker(Func<Card, int> ranker, Func<Card, bool> filter = null, Controller controller = Controller.Opponent)
    {
      filter = filter ?? delegate { return true; };
      
      return (target, source, maxX, game) =>
        {
          var ctrl = controller == Controller.Opponent
            ? game.Players.GetOpponent(source.Controller)
            : source.Controller;

          if (target.Card().Controller != ctrl)
            return WellKnownTargetScores.NotAccepted;
          
          if (!filter(target.Card()))
            return WellKnownTargetScores.NotAccepted;

          return WellKnownTargetScores.Good + ranker(target.Card());
        };
    }

    public static Core.ScoreCalculator BattlefieldScoreRanker(Func<Card, bool> filter = null, Controller controller = Controller.Opponent)
    {
      return BattlefieldRanker(ranker: (card) => card.Score, filter: filter, controller: controller);
    }

    public static Core.ScoreCalculator PreferTopSpellTarget()
    {
      return (target, source, maxX, game) =>
        {
          if (game.Stack.TopSpell == null || game.Stack.TopSpell.Target == null)
            return WellKnownTargetScores.Good;
          
          return target == game.Stack.TopSpell.Target
            ? WellKnownTargetScores.Good
            : WellKnownTargetScores.NotAccepted;
        };
    }

    public enum Controller
    {
      SpellOwner,
      Opponent      
    }
        
  }
}