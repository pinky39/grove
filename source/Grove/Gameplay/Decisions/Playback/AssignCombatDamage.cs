namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class AssignCombatDamage : Decisions.AssignCombatDamage
  {
    protected override bool ShouldExecuteQuery { get { return true; } }  

    protected override void ExecuteQuery()
    {
      Result = (DamageDistribution) Game.Recorder.LoadDecisionResult();
    }

    public override void SaveDecisionResults() {}
  }
}