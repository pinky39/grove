namespace Grove.UserInterface.CombatDamage
{
  using System;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly BlockerDamageAssignments _assignments;
    private readonly Attacker _attacker;
    private readonly DamageDistribution _damageDistribution;
    private int _damageLeftToAssign;
    private int _totalDamageToAssign;

    public ViewModel(Attacker attacker, DamageDistribution damageDistribution)
    {
      _damageDistribution = damageDistribution;
      _assignments = new BlockerDamageAssignments(attacker);
      _attacker = attacker;
      _totalDamageToAssign = attacker.Card.CalculateCombatDamageAmount();
      _damageLeftToAssign = _totalDamageToAssign;
    }

    public Card Attacker { get { return _attacker.Card; } }
    public BlockerDamageAssignments Blockers { get { return _assignments; } }

    public bool CanAccept
    {
      get
      {
        return HasAllDamageBeenAssigned || _attacker.AssignsDamageAsThoughItWasntBlocked ||
          (_attacker.HasTrample && _assignments.All(x => x.HasAssignedLeathalDamage));
      }
    }

    private bool HasAllDamageBeenAssigned { get { return _damageLeftToAssign == 0; } }

    public string Title { get { return String.Format("Distribute combat damage: {0} left.", _damageLeftToAssign); } }

    public void Accept()
    {
      foreach (var assignment in _assignments)
      {
        _damageDistribution.Assign(assignment.Blocker, assignment.AssignedDamage);
      }

      Close();
    }

    [Updates("CanAccept", "Title")]
    public virtual void AssignOneDamage(BlockerDamageAssignment blocker)
    {
      if (!HasAllDamageBeenAssigned && _assignments.CanAssignCombatDamageTo(blocker))
      {
        _damageLeftToAssign -= 1;
        blocker.AssignedDamage++;
      }
    }
    
    [Updates("CanAccept", "Title")]
    public virtual void Clear()
    {
      _assignments.Clear();
      _damageLeftToAssign = _totalDamageToAssign;
    }

    public virtual void Close() {}


    public interface IFactory
    {
      ViewModel Create(Attacker attacker, DamageDistribution damageDistribution);
    }
  }
}