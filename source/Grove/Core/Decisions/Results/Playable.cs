namespace Grove.Core.Decisions.Results
{
  using Grove.Infrastructure;

  [Copyable]
  public abstract class Playable
  {
    public virtual bool WasPriorityPassed { get { return false; } }

    public virtual bool CanPlay()
    {
      return true;
    }

    public virtual void Play() {}
  }
}