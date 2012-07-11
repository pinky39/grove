namespace Grove.Core.Controllers
{
  using Results;

  public abstract class TakeMulligan : Decision<BooleanResult>
  {
    protected override bool ShouldExecuteQuery { get { return Player.CanMulligan; } }

    public override void ProcessResults()
    {
      if (ShouldExecuteQuery == false)
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
  }
}