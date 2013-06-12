namespace Grove.Artifical.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Decisions.Results;
  using Infrastructure;

  public class SetDamageAssignmentOrder : Gameplay.Decisions.SetDamageAssignmentOrder
  {
    public SetDamageAssignmentOrder()
    {
      Result = new DamageAssignmentOrder();
    }
    
    protected override void ExecuteQuery()
    {
      if (Attacker.HasDeathTouch)
      {
        Result = DeathTouchScenario();
        return;
      }

      Result = DefaultScenario();
    }

    private DamageAssignmentOrder DeathTouchScenario()
    {
      var damageAssignmentOrder = new DamageAssignmentOrder();

      var orderedByScore = Attacker.Blockers
        .OrderByDescending(blocker => blocker.Score)
        .ToList();

      for (var i = 0; i < orderedByScore.Count; i++)
      {
        damageAssignmentOrder.Assign(orderedByScore[i], i);
      }

      return damageAssignmentOrder;
    }

    private DamageAssignmentOrder DefaultScenario()
    {
      var damageAssignmentOrder = new DamageAssignmentOrder();

      var blockers = GetBlockersThatCanBeDealtLeathalDamageProducingTheGreatestScore();
      blockers = IncludeOtherBlockersAfter(blockers);

      for (var i = 0; i < blockers.Count; i++)
      {
        damageAssignmentOrder.Assign(blockers[i], i);
      }

      return damageAssignmentOrder;
    }

    private List<Blocker> GetBlockersThatCanBeDealtLeathalDamageProducingTheGreatestScore()
    {
      var candidates = Attacker.Blockers
        .Where(blocker => blocker.LifepointsLeft > 0)
        .Where(blocker => blocker.LifepointsLeft <= Attacker.Card.CalculateCombatDamageAmount())
        .Select(blocker => new KnapsackItem<Blocker>(
          item: blocker,
          weight: blocker.LifepointsLeft,
          value: blocker.Score))
        .ToList();

      var result = Knapsack.Solve(candidates, Attacker.Card.CalculateCombatDamageAmount());
      return result.OrderByDescending(x => x.Value).Select(x => x.Item).ToList();
    }

    private List<Blocker> IncludeOtherBlockersAfter(List<Blocker> blockers)
    {
      blockers.AddRange(
        Attacker.Blockers
          .Where(blocker => !blockers.Contains(blocker))
          .OrderByDescending(blocker => blocker.Score)
        );

      return blockers;
    }
  }
}