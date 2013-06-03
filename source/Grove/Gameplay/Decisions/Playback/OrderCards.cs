namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class OrderCards : Decisions.OrderCards
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (Ordering) Game.Recorder.LoadDecisionResult();
    }
  }
}