namespace Grove.Core.Controllers
{
  using Results;

  public abstract class DeclareAttackers : Decision<ChosenCards>
  {
    public Combat Combat { get; set; }

    public override void ProcessResults()
    {
      if (Result == null)
        return;

      foreach (var attacker in Result)
      {
        Combat.DeclareAttacker(attacker);
      }
    }
    
    protected override bool ShouldExecuteQuery
    {
      get { return Player.Battlefield.HasCreaturesThatCanAttack; }
    }  
  }
}