namespace Grove.Gameplay.Decisions.Playback
{
  using System;
  using Results;

  public class ChooseToUntap : Decisions.ChooseToUntap
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (BooleanResult) Game.LoadDecisionResult();
    }
  }
}