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
          return Convert.ToInt32(RankBounds.BestRank - (target.Card().Power + target.Card().Toughness));

        return RankBounds.BestRank - target.Card().ManaCost.Count();
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
            return considerSpellController ? RankBounds.WorseRank : RankBounds.NotAcceptedRank;
          }

          if (target.Card().Has().Indestructible && spellsDamage > 0 && !reducesPwt)
          {
            return RankBounds.NotAcceptedRank;
          }
          
          var score = target.Card().Score;

          if (spellsDamage > 0 && target.Card().LifepointsLeft > spellsDamage)
          {
            score = score/2;
          }

          return RankBounds.BestRank + score;
        }

        if (target.IsEffect())
        {
          if (target.Effect().Controller != game.Players.GetOpponent(source.Controller))
            return considerSpellController
              ? RankBounds.WorseRank
              : RankBounds.NotAcceptedRank;

          return RankBounds.BestRank;
        }

        if (target.Player() == opponent)
        {
          var rank = RankBounds.BestRank;
                    
          if (spellsDamage > 0 || maxX > 0)
          {
            var damage = spellsDamage ?? maxX;
            rank += ScoreCalculator.CalculateLifelossScore(opponent.Life, damage.Value);
          }

          return rank;
        }

        return RankBounds.NotAcceptedRank;
      };
    }

    public static Core.ScoreCalculator YourStuffScoresMore()
    {
      return (target, source, maxX, game) => {
        var opponent = game.Players.GetOpponent(source.Controller);

        if (target.IsCard())
        {
          return target.Card().Controller == opponent
            ? RankBounds.NotAcceptedRank
            : RankBounds.BestRank;
        }

        return target.Player() != opponent
          ? RankBounds.BestRank
          : RankBounds.NotAcceptedRank;
      };
    }
  }
}