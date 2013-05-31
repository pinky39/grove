namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class DeclareAttackers : Decision<ChosenCards>
  {
    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.HasCreaturesThatCanAttack; } }

    protected override void SetResultNoQuery()
    {
      Result = new ChosenCards();
    }

    public override void ProcessResults()
    {      
      foreach (var attacker in Result)
      {
        Game.Combat.DeclareAttacker(attacker);
      }
    }
  }
}