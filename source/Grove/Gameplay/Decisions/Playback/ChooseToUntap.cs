namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class ChooseToUntap : Decisions.ChooseToUntap
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (BooleanResult) Game.Recorder.LoadDecisionResult();
    }
  }
}