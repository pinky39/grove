namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class SetTriggeredAbilityTarget : Decisions.SetTriggeredAbilityTarget
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (ChosenTargets) Game.Recorder.LoadDecisionResult();
    }
  }
}