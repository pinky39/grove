namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using System.Linq;

  public static class TargetFilters
  {
    public delegate IEnumerable<ITarget> InputSelectorDelegate(TargetsCandidates candidates);
    public delegate IEnumerable<Targets> OutputSelectorDelegate(IEnumerable<ITarget> targets);

    public static TargetsFilterDelegate PermanentsByDescendingScore(Controller controller = Controller.Opponent)
    {
      return p =>
        {
          var candidates = p.Candidates();

          switch (controller)
          {
            case Controller.Opponent:
              candidates = candidates
                .RestrictController(p.Opponent);
              break;
            case Controller.SpellOwner:
              candidates = candidates
                .RestrictController(p.Controller);
              break;
          }

          return p.Targets(candidates
            .OrderByDescending(x => x.Card().Score));
        };
    }

    public static TargetsFilterDelegate Opponent()
    {
      return p =>
        {
          return p.Targets(p.Candidates()
            .Where(x => x.Player() == p.Opponent));
        };
    }

    public static TargetsFilterDelegate PumpAttackerOrBlocker(int power, int thougness)
    {
      return p =>
        {
          var candidates = p.Candidates().RestrictController(p.Controller)
            .Select(
              x =>
                new
                  {
                    Card = x.Card(),
                    Gain = p.Combat.CalculateGainIfGivenABoost(x.Card(), power, thougness)
                  })
            .Where(x => x.Gain > 0)
            .OrderByDescending(x => x.Gain)
            .Select(x => x.Card);

          return p.Targets(candidates);
        };
    }

    public static TargetsFilterDelegate CounterSpell()
    {
      return p =>
        {
          var candidates = p.Candidates().RestrictController(p.Opponent)
            .Take(1);

          return p.Targets(candidates);
        };
    }

    public static TargetsFilterDelegate CombatAttachment()
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

    public static TargetsFilterDelegate DealDamage(int? amount = null)
    {
      return p =>
        {
          amount = amount ?? p.MaxX;

          var candidates = p.Candidates()
            .Where(x => x == p.Opponent)
            .Select(x => new
              {
                Target = x,
                Score = ScoreCalculator.CalculateLifelossScore(x.Player().Life, amount.Value)
              })
            .Concat(
              p.Candidates()
                .Where(x => x.IsCard() && x.Card().Controller == p.Opponent)
                .Select(x => new
                  {
                    Target = x,
                    Score = x.Card().LifepointsLeft <= amount ? x.Card().Score : 0
                  }))
            .OrderByDescending(x => x.Score)
            .Select(x => x.Target);

          return p.Targets(candidates);
        };
    }

    private static int CalculateAttackerScore(Card card, Combat combat)
    {
      return combat.CouldBeBlockedByAny(card) ? 5 : 0 + card.Power.Value;
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

    public static TargetsFilterDelegate CombatEnchantment()
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
  }
}