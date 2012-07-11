namespace Grove.Core.Controllers
{
  using Details.Combat;
  using Results;

  public abstract class SetDamageAssignmentOrder : Decision<DamageAssignmentOrder>
  {
    public Attacker Attacker { get; set; }

    protected override bool ShouldExecuteQuery { get { return Attacker.BlockersCount > 1; } }

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
      {
        return;
      }

      Attacker.SetDamageAssignmentOrder(Result);
    }
  }
}