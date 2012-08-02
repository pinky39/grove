namespace Grove.Core.Controllers
{
  using System;
  using System.Collections.Generic;

  public abstract class AdhocDecision<T> : Decision<T> where T : class
  {
    private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

    public Action<AdhocDecision<T>> Process = delegate { };
    public Func<Machine.AdhocDecision<T>, T> QueryAi = (self) => default(T);
    public Func<Human.AdhocDecision<T>, T> QueryUi = (shell) => default(T);

    public void Param(string key, object value)
    {
      _parameters[key] = value;
    }

    public TParam Param<TParam>(string key)
    {
      return (TParam) _parameters[key];
    }

    public override void ProcessResults()
    {
      Process(this);
    }
  }
}