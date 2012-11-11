namespace Grove.Core.Controllers.Results
{
  using Infrastructure;

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