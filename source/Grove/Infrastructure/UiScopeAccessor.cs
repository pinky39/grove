namespace Grove.Infrastructure
{
  using Castle.MicroKernel.Context;
  using Castle.MicroKernel.Lifestyle.Scoped;

  public class UiScopeAccessor : IScopeAccessor
  {
    public void Dispose()
    {
      Bootstrapper.NewGame();
    }

    public ILifetimeScope GetScope(CreationContext context)
    {
      return Bootstrapper.GetScope();
    }
  }  
}