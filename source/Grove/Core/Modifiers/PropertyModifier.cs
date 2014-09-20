namespace Grove.Modifiers
{
  using Grove.Infrastructure;

  [Copyable]
  public abstract class PropertyModifier<TValue>
  {
    public TrackableEvent Changed;

    protected PropertyModifier()
    {
      Changed = new TrackableEvent(this);
    }

    public abstract int Priority { get; }
    public abstract TValue Apply(TValue before);

    public virtual void Initialize(INotifyChangeTracker changeTracker)
    {
      Changed.Initialize(changeTracker);
    }

    protected void NotifyModifierHasChanged()
    {
      Changed.Raise();
    }
  }
}