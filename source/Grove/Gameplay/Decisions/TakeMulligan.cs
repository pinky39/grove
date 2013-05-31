namespace Grove.Gameplay.Decisions
{
  using Results;

  public abstract class TakeMulligan : Decision<BooleanResult>
  {
    protected override bool ShouldExecuteQuery { get { return Controller.CanMulligan; } }

    protected override void SetResultNoQuery()
    {
      Result = false;
    }
    
    public override void ProcessResults()
    {      
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