namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class TakeMulligan : Decisions.TakeMulligan
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (BooleanResult) Game.LoadDecisionResult();
    }
  }
}