namespace Grove.Ui.Notification
{
  using System;
  using System.Timers;
  using Caliburn.Micro;
  using Infrastructure;

  public class ViewModel : IClosable
  {
    private readonly Timer _timer = new Timer();

    public ViewModel(string message)
    {
      Message = message;
      AutoCloseIn(5);
    }

    public string Message { get; private set; }

    public event EventHandler Closed = delegate { };

    public void Close()
    {
      _timer.Stop();
      _timer.Dispose();
    }

    private void AutoCloseIn(int numOfSeconds)
    {
      _timer.Interval = numOfSeconds*1000;

      _timer.Elapsed += delegate { Execute.OnUIThread(Close); };

      _timer.Start();
    }
  }
}