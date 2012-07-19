namespace Grove.Core.Controllers
{
  using System;
  using System.Collections.Generic;

  public abstract class AdHocDecision<T> : Decision<T> where T : class
  {
    private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

    public Action<AdHocDecision<T>> Process = delegate { };
    public Func<Machine.AdHocDecision<T>, T> QueryAi = (self) => default(T);
    public Func<Human.AdHocDecision<T>, T> QueryUi = (shell) => default(T);

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