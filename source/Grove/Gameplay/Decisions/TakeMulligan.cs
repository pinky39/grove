namespace Grove.Gameplay.Decisions
{
  using System;
  using Results;

  [Serializable]
  public abstract class TakeMulligan : Decision<BooleanResult>
  {
    protected override bool ShouldExecuteQuery { get { return Controller.CanMulligan; } }

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
        return;

      if (Result.IsTrue)
      {
        Controller.TakeMulligan();
      }
      else
      {
        Controller.HasMulligan = false;
      }
    }
  }
}