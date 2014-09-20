namespace Grove
{
  using Grove.Infrastructure;
  using Grove.Modifiers;

  [Copyable]
  public class SkipSteps : IAcceptsPlayerModifier
  {
    private readonly TrackableList<Step> _steps = new TrackableList<Step>();

    public void Accept(IPlayerModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker)
    {
      _steps.Initialize(changeTracker);
    }

    public bool Contains(Step step)
    {
      return _steps.Contains(step);
    }

    public void Add(Step step)
    {
      _steps.Add(step);
    }

    public void Remove(Step step)
    {
      _steps.Remove(step);
    }
  }
}