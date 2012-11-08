namespace Grove.Core.Controllers.Machine
{
  using System;

  public class AdhocDecision<T> : Controllers.AdhocDecision<T> where T : class
  {        
    protected override void ExecuteQuery()
    {
      Result = QueryAi(this);
    }
  }
}