namespace Grove.Core.Controllers
{
  using Results;

  public abstract class DeclareAttackers : Decision<ChosenCards>
  {
    public Combat Combat { get; set; }

    protected override bool ShouldExecuteQuery { get { return Player.Battlefield.HasCreaturesThatCanAttack; } }

    public override void ProcessResults()
    {
      if (Result == null)
        return;

      foreach (var attacker in Result)
      {
        Combat.DeclareAttacker(attacker);
      }
    }
  }
}