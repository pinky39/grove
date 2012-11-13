namespace Grove.Core.Cards
{
  using Grove.Infrastructure;

  [Copyable]
  public abstract class PropertyModifier<TValue>
  {
    public TrackableEvent Changed;

    protected PropertyModifier(ChangeTracker changeTracker)
    {
      Changed = new TrackableEvent(this, changeTracker);
    }

    public abstract int Priority { get; }
    public abstract TValue Apply(TValue before);

    protected void NotifyModifierHasChanged()
    {
      Changed.Raise();
    }
  }
}