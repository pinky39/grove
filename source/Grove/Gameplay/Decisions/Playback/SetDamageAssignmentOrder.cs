namespace Grove.Gameplay.Decisions.Playback
{
  using System;
  using Results;

  public class SetDamageAssignmentOrder : Decisions.SetDamageAssignmentOrder
  {
    protected override bool ShouldExecuteQuery
    {
      get { return true; }
    }
    
    protected override void ExecuteQuery()
    {
      Result = (DamageAssignmentOrder) Game.Recorder.LoadDecisionResult();
    }

    public override void SaveDecisionResults() {}
  }
}