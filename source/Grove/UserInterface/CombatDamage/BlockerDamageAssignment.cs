namespace Grove.UserInterface.CombatDamage
{
  using Gameplay;

  public class BlockerDamageAssignment
  {
    public BlockerDamageAssignment(Blocker blocker)
    {
      Blocker = blocker;
    }

    public Blocker Blocker { get; set; }


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