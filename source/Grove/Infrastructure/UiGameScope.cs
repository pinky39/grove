namespace Grove.Infrastructure
{
  using Castle.MicroKernel.Context;
  using Castle.MicroKernel.Lifestyle.Scoped;

  public class UiGameScope : IScopeAccessor
  {
    private static ILifetimeScope _scope;

    public ILifetimeScope GetScope(CreationContext context)
    {
      return _scope ?? (_scope = new DefaultLifetimeScope());
    }

    public void Dispose()
    {
      DisposeScope();
    }

    private static void DisposeScope()
    {
      if (_scope != null)
        _scope.Dispose();

      _scope = null;
    }

    public static void Reset()
    {
      DisposeScope();      
    }
  }
}