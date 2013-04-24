namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class DeclareAttackers : Decision<ChosenCards>
  {    
    protected override bool ShouldExecuteQuery { get { return Controller.Battlefield.HasCreaturesThatCanAttack; } }

    public override void ProcessResults()
    {
      if (Result == null)
        return;

      foreach (var attacker in Result)
      {
        Game.Combat.DeclareAttacker(attacker);
      }
    }
  }
}