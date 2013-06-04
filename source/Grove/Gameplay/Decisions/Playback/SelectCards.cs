namespace Grove.Gameplay.Decisions.Playback
{
  using Results;

  public class SelectCards : Decisions.SelectCards
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (ChosenCards) Game.Recorder.LoadDecisionResult();
    }

    public override void SaveDecisionResults() {}
  }
}