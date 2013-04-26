namespace Grove.Ui.CombatDamage
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Combat;
  using Infrastructure;

  public class BlockerDamageAssignments : IEnumerable<BlockerDamageAssignment>
  {
    private readonly List<BlockerDamageAssignment> _assignments;

    public BlockerDamageAssignments(Attacker attacker)
    {
      _assignments = attacker.Blockers
        .OrderBy(x => x.DamageAssignmentOrder)
        .Select(x => Bindable.Create<BlockerDamageAssignment>(x))
        .ToList();
    }


    public IEnumerator<BlockerDamageAssignment> GetEnumerator()
    {
      return _assignments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public bool CanAssignCombatDamageTo(BlockerDamageAssignment blocker)
    {
      foreach (var assignment in _assignments)
      {
        if (assignment == blocker)
          return true;

        if (assignment.HasAssignedLeathalDamage == false)
          return false;
      }

      return false;
    }

    public void Clear()
    {
      foreach (var blocker in _assignments)
      {
        blocker.AssignedDamage = 0;
      }
    }
  }
}