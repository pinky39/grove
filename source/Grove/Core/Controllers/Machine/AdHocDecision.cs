namespace Grove.Core.Controllers.Machine
{
  public class AdhocDecision<T> : Controllers.AdhocDecision<T> where T : class
  {        
    protected override void ExecuteQuery()
    {
      Result = QueryAi(this);
    }
  }
}