namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class ChooseTo : Decisions.ChooseTo
  {
    protected override bool ShouldExecuteQuery { get { return true; } }  

    protected override void ExecuteQuery()
    {
      Result = (BooleanResult) Game.Recorder.LoadDecisionResult();
    }

    public override void SaveDecisionResults() {}
  }
}