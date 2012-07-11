namespace Grove.Ui.CombatDamage
{
  using Core.Details.Combat;

  public class BlockerDamageAssignment
  {
    public BlockerDamageAssignment(Blocker blocker)
    {
      Blocker = blocker;
    }

    public Blocker Blocker { get; private set; }
    public virtual int Damage { get; set; }
  }
}