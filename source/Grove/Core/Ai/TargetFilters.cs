namespace Grove.Core.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static class TargetFilters
  {
    public static TargetsFilterDelegate PermanentsByDescendingScore(Controller controller = Controller.Opponent)
    {
      return p =>
        {
          IEnumerable<TargetCandidate> candidates = p.Candidates.Effect;

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

          return candidates
            .OrderByDescending(x => x.Target.Card().Score)
            .Select(x => new Targets {Effect = x.Target})
            .ToList();
        };
    }

    public static TargetsFilterDelegate Opponent()
    {
      return p =>
        {
          return p.Candidates.Effect
            .Where(x => x.Target.Player() == p.Opponent)
            .Select(x => new Targets {Effect = x.Target})
            .ToList();
        };
    }

    public static TargetsFilterDelegate PumpAttackerOrBlocker(int power, int thougness)
    {
      return p =>
        {
          var candidates = p.Candidates.Effect.RestrictController(p.Controller)
            .Select(
              x =>
                new
                  {
                    Card = x.Target.Card(),
                    Gain = p.Combat.CalculateGainIfGivenABoost(x.Target.Card(), power, thougness)
                  })
            .Where(x => x.Gain > 0)
            .OrderByDescending(x => x.Gain)
            .Select(x => new Targets {Effect = x.Card})
            .ToList();

          return candidates;
        };
    }

    public static TargetsFilterDelegate CounterSpell()
    {
      return p =>
        {
          var candidates = p.Candidates.Effect.RestrictController(p.Opponent)
            .Take(1)
            .Select(x => new Targets {Effect = x.Target})
            .ToList();

          return candidates;
        };
    }

    public static TargetsFilterDelegate CombatAttachment()
    {
      return p =>
        {
          var candidates = p.Candidates.Effect;
            .Select()



        };
    }
  }
}