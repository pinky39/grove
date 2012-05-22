namespace Grove.Ui.CombatDamage
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Infrastructure;

  public class BlockerDamageAssignments : IEnumerable<BlockerDamageAssignment>
  {
    private readonly List<BlockerDamageAssignment> _assignments;
    private readonly Attacker _attacker;

    public BlockerDamageAssignments(Attacker attacker)
    {
      _attacker = attacker;

      _assignments = attacker.Blockers
        .Select(x => Bindable.Create<BlockerDamageAssignment>(x))
        .OrderBy<BlockerDamageAssignment, int>(x=>x.Blocker.DamageAssignmentOrder)
        .ToList<BlockerDamageAssignment>();
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
      if (_attacker.HasDeathTouch)
        return true;

      var ordered = _assignments.OrderBy(x => x.Blocker.DamageAssignmentOrder);

      foreach (var assignment in ordered)
      {
        if (assignment == blocker)
          return true;

        if (assignment.Blocker.HasAssignedLeathalDamage == false)
          return false;
      }

      return false;
    }

    public void Clear()
    {
      foreach (var blocker in _assignments)
      {
        blocker.Damage = 0;
      }
    }
  }
}