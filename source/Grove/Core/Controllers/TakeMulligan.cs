namespace Grove.Core.Controllers
{
  using Results;

  public abstract class TakeMulligan : Decision<BooleanResult>
  {
    public override void ProcessResults()
    {
      if(ShouldExecuteQuery == false)
        return;

      if (Result.IsTrue)
      {
        Player.TakeMulligan();
      }
      else
      {
        Player.HasMulligan = false;
      }
    }

    protected override bool ShouldExecuteQuery
    {
      get { return Player.CanMulligan; }
    }
  }
}