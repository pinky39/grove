namespace Grove.Core.Controllers.Machine
{
  public class AdHocDecision<T> : Controllers.AdHocDecision<T> where T : class
  {        
    protected override void ExecuteQuery()
    {
      Result = QueryAi(this);
    }
  }
}