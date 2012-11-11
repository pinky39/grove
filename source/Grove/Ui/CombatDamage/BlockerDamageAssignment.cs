namespace Grove.Ui.CombatDamage
{
  using System;
  using Core;

  public class BlockerDamageAssignment
  {
    public BlockerDamageAssignment(Blocker blocker)
    {
      Blocker = blocker;
    }

    public Blocker Blocker { get; private set; }
    public virtual int Damage { get; set; }
    
    public bool HasAssignedLeathalDamage { get
    {
      if (Blocker.Attacker.HasDeathTouch)
        return Damage > 0;

      return Blocker.LifepointsLeft <= Damage;
    } }
  }
}