namespace Grove.Gameplay.Decisions.Playback
{
  using System;
  using Results;

  public class DeclareBlockers : Decisions.DeclareBlockers
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (ChosenBlockers) Game.Recorder.LoadDecisionResult();
    }
  }
}