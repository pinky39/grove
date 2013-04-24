namespace Grove.Ai.Decisions
{
  public class PayOr : Gameplay.Decisions.PayOr
  {
    protected override void ExecuteQuery()
    {
      Result = Ai(this);
    }
  }
}