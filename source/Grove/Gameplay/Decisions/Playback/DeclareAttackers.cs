namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class DeclareAttackers : Decisions.DeclareAttackers
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (ChosenCards) Game.Recorder.LoadDecisionResult();
    }
  }
}