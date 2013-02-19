namespace Grove.Core
{
  using Infrastructure;

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

    public virtual void Initialize(ChangeTracker changeTracker)
    {
      Changed.Initialize(changeTracker);
    }

    protected void NotifyModifierHasChanged()
    {
      Changed.Raise();
    }
  }
}