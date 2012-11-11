namespace Grove.Core.Controllers.Machine
{
  public class PayOr : Controllers.PayOr
  {
    protected override void ExecuteQuery()
    {
      Result = Ai(this);
    }
  }
}