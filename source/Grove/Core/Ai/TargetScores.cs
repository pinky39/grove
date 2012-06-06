namespace Grove.Core.Ai
{
  using System;
  using System.Linq;

  public static class TargetScores
  {
    public static Core.ScoreCalculator LessValuableCardsScoreMore()
    {
      return (target, source, maxX, game) => {
        if (target.Card().Is().Token)
          return Convert.ToInt32(Core.WellKnownTargetScores.Good - (target.Card().Power + target.Card().Toughness));

        return Core.WellKnownTargetScores.Good - target.Card().ManaCost.Count();
      };
    }

    public static Core.ScoreCalculator OpponentStuffScoresMore(int? spellsDamage = null, bool considerSpellController = false, bool reducesPwt = false)
    {
      return (target, source, maxX, game) => {
        var opponent = game.Players.GetOpponent(source.Controller);

        if (target.IsCard())
        {
          var controller = target.Card().Controller;

          if (controller != opponent)
          {
            return considerSpellController ? Core.WellKnownTargetScores.Bad : Core.WellKnownTargetScores.NotAccepted;
          }

          if (target.Card().Has().Indestructible && spellsDamage > 0 && !reducesPwt)
          {
            return Core.WellKnownTargetScores.NotAccepted;
          }
          
          var score = target.Card().Score;

          if (spellsDamage > 0 && target.Card().LifepointsLeft > spellsDamage)
          {
            score = score/2;
          }

          return Core.WellKnownTargetScores.Good + score;
        }

        if (target.IsEffect())
        {
          if (target.Effect().Controller != game.Players.GetOpponent(source.Controller))
            return considerSpellController
              ? Core.WellKnownTargetScores.Bad
              : Core.WellKnownTargetScores.NotAccepted;

          return Core.WellKnownTargetScores.Good;
        }

        if (target.Player() == opponent)
        {
          var rank = Core.WellKnownTargetScores.Good;
                    
          if (spellsDamage > 0 || maxX > 0)
          {
            var damage = spellsDamage ?? maxX;
            rank += ScoreCalculator.CalculateLifelossScore(opponent.Life, damage.Value);
          }

          return rank;
        }

        return Core.WellKnownTargetScores.NotAccepted;
      };
    }

    public static Core.ScoreCalculator YourStuffScoresMore()
    {
      return (target, source, maxX, game) => {
        var opponent = game.Players.GetOpponent(source.Controller);

        if (target.IsCard())
        {
          return target.Card().Controller == opponent
            ? Core.WellKnownTargetScores.NotAccepted
            : Core.WellKnownTargetScores.Good;
        }

        return target.Player() != opponent
          ? Core.WellKnownTargetScores.Good
          : Core.WellKnownTargetScores.NotAccepted;
      };
    }
  }
}