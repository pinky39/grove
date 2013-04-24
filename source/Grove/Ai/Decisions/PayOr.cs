namespace Grove.Core.Decisions.Machine
{
  public class PayOr : Decisions.PayOr
  {
    protected override void ExecuteQuery()
    {
      Result = Ai(this);
    }
  }
}