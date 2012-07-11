namespace Grove.Core.Details.Cards.Modifiers
{
  using System;
  using Infrastructure;

  public class DependantLifetime : Lifetime, ICopyContributor
  {
    private readonly ILifetimeDependency _lifetimeDependency;

    private DependantLifetime() {}

    public DependantLifetime(Modifier modifier, ILifetimeDependency lifetimeDependency, ChangeTracker changeTracker)
      : base(modifier, changeTracker)
    {
      _lifetimeDependency = lifetimeDependency;
      _lifetimeDependency.EndOfLife += OnEndOfLife;
    }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      _lifetimeDependency.EndOfLife += OnEndOfLife;
    }

    private void OnEndOfLife(object sender, EventArgs e)
    {
      End();
    }

    public override void Dispose()
    {
      _lifetimeDependency.EndOfLife -= OnEndOfLife;
    }
  }
}