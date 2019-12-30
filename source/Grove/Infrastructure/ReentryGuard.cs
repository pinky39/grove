namespace Grove.Infrastructure
{
  using System;
  
  [Copyable]
  public class ReentryGuard : IDisposable
  {
    private bool _set;
    
    public bool IsSet
    {
      get { return _set; }
    }

    public void Dispose()
    {
      _set = false;
    }

    public IDisposable Set()
    {
      _set = true;
      return this;
    }
  }
}