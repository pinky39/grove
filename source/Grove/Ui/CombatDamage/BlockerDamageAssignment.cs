namespace Grove.Ui.CombatDamage
{
  using Core;

  public class BlockerDamageAssignment
  {
    public Blocker Blocker { get; set; }

    public BlockerDamageAssignment(Blocker blocker)
    {
      Blocker = blocker;
    }


    public virtual int AssignedDamage { get; set; }

    public bool HasAssignedLeathalDamage
    {
      get
      {
        if (Blocker.Attacker.HasDeathTouch)
          return AssignedDamage > 0;

        return Blocker.LifepointsLeft <= AssignedDamage;
      }
    }
  }
}