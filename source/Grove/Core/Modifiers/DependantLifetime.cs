namespace Grove.Core.Modifiers
{
  using System;
  using Grove.Infrastructure;

  public class DependantLifetime : Lifetime, ICopyContributor
  {
    private ILifetimeDependency _lifetimeDependency;

    public ILifetimeDependency LifetimeDependency
    {
      get { return _lifetimeDependency; }
      set
      {
        _lifetimeDependency = value;
        _lifetimeDependency.EndOfLife += OnEndOfLife;
      }
    }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      LifetimeDependency.EndOfLife += OnEndOfLife;
    }

    private void OnEndOfLife(object sender, EventArgs e)
    {
      End();
    }

    public override void Dispose()
    {
      LifetimeDependency.EndOfLife -= OnEndOfLife;
    }
  }
}