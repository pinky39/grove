namespace Grove.Gameplay.Decisions.Playback
{
  using System;
  using Results;

  public class SelectCards : Decisions.SelectCards
  {
    protected override bool ShouldExecuteQuery
    {
      get { return true; }
    }
    
    protected override void ExecuteQuery()
    {
      Result = (ChosenCards) Game.Recorder.LoadDecisionResult();
    }
  }
}