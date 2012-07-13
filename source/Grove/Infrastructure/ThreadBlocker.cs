namespace Grove.Infrastructure
{
  using System.Diagnostics;
  using System.Threading;
  using System.Windows.Threading;
  using Caliburn.Micro;

  public class ThreadBlocker
  {
    private AutoResetEvent _autoResetEvent;
    private DispatcherFrame _frame;

    public void BlockUntilCompleted(System.Action action = null)
    {
      action = action ?? delegate { };
      
      var result = BlockOnUiThread(action) ??
        BlockOnWorkerThread(action);
    }

    public void Completed()
    {
      if (_frame != null)
      {
        _frame.Continue = false;
      }

      if (_autoResetEvent != null)
        _autoResetEvent.Set();
    }

    private object BlockOnUiThread(System.Action action)
    {
      var dispather = Dispatcher.CurrentDispatcher;

      if (!dispather.CheckAccess())
        return Chaining.Continue;

      Debug.Assert(_frame == null, "Do not reuse existing thread blocker, create a new one instead.");
      _frame = new DispatcherFrame(true);

      action();

      // this will block until completed is called
      Dispatcher.PushFrame(_frame);
      _frame = null;

      return Chaining.Stop;
    }

    private object BlockOnWorkerThread(System.Action action)
    {
      Debug.Assert(_autoResetEvent == null, "Do not reuse existing thread blocker, create a new one instead.");
      _autoResetEvent = new AutoResetEvent(false);

      Execute.OnUIThread(action);

      _autoResetEvent.WaitOne();
      _autoResetEvent = null;

      return Chaining.Stop;
    }
  }
}