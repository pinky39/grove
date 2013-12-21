namespace Grove.UserInterface
{
  using System;
  using Infrastructure;

  public class Animation
  {
    private EventHandler _finished = delegate { };
    private ThreadBlocker _threadBlocker;

    public virtual bool ShouldStart { get; protected set; }

    public void Stop()
    {
      _finished(this, EventArgs.Empty);
    }

    public void Start()
    {
      ShouldStart = true;

      _threadBlocker = new ThreadBlocker();
      _threadBlocker.BlockUntilCompleted(() => { _finished += delegate { _threadBlocker.Completed(); }; });
    }

    public static Animation Create()
    {
      return Bindable.Create<Animation>();
    }
  }
}