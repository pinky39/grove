namespace Grove.Gameplay.Decisions
{
  using System.Linq;
  using Results;

  public abstract class SetDamageAssignmentOrder : Decision<DamageAssignmentOrder>
  {
    public Attacker Attacker { get; set; }

    protected override bool ShouldExecuteQuery { get { return Attacker.BlockersCount > 1; } }

    protected override void SetResultNoQuery()
    {
      Result = new DamageAssignmentOrder();
      
      if (Attacker.BlockersCount == 0)
        return;
      
      Result.Assign(Attacker.Blockers.First(), 1);      
    }
    
    public override void ProcessResults()
    {
      Attacker.SetDamageAssignmentOrder(Result);
    }
  }
}