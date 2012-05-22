namespace Grove.Core.Controllers
{
  using Results;

  public abstract class SetDamageAssignmentOrder : Decision<DamageAssignmentOrder>
  {
    public Attacker Attacker { get; set; }

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
      {        
        return;
      }      
      
      Attacker.SetDamageAssignmentOrder(Result);
    }

    protected override bool ShouldExecuteQuery
    {
      get { return Attacker.BlockersCount > 1; }
    }    
  }
}