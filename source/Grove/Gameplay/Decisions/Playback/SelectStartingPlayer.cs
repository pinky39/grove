namespace Grove.Gameplay.Decisions.Playback
{
  using System;
  using Results;

  public class SelectStartingPlayer : Decisions.SelectStartingPlayer
  {
    protected override bool ShouldExecuteQuery
    {
      get { return true; }
    }
    
    protected override void ExecuteQuery()
    {
      Result = (ChosenPlayer) Game.Recorder.LoadDecisionResult();
    }
  }
}