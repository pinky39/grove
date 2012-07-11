namespace Grove.Ui.DamageOrder
{
  using Core.Details.Combat;

  public class BlockerAssignment
  {
    public BlockerAssignment(Blocker blocker)
    {
      Blocker = blocker;
    }

    public virtual int? AssignmentOrder { get; set; }
    public Blocker Blocker { get; private set; }
  }
}