namespace Grove.UserInterface.ManaPool
{
  using System;
  using System.Threading;

  public class ViewModel : ViewModelBase, IDisposable
  {
    private Timer _timer;
    
    public override void Initialize()
    {
      _timer = new Timer(delegate { Update(); }, null,
        TimeSpan.FromMilliseconds(20),
        TimeSpan.FromMilliseconds(20));
    }

    public virtual int WhiteCount { get; protected set; }
    public virtual int BlueCount { get; protected set; }
    public virtual int BlackCount { get; protected set; }
    public virtual int RedCount { get; protected set; }
    public virtual int GreenCount { get; protected set; }
    public virtual int ColorlessCount { get; protected set; }
    public virtual int MultiCount { get; protected set; }

    public void Dispose()
    {
      _timer.Dispose();
    }

    private void Update()
    {
      var pool = Players.Human.ManaPool;

      Update(() => WhiteCount != pool.White, () => WhiteCount = pool.White);
      Update(() => BlueCount != pool.Blue, () => BlueCount = pool.Blue);
      Update(() => BlackCount != pool.Black, () => BlackCount = pool.Black);
      Update(() => RedCount != pool.Red, () => RedCount = pool.Red);
      Update(() => GreenCount != pool.Green, () => GreenCount = pool.Green);
      Update(() => ColorlessCount != pool.Colorless, () => ColorlessCount = pool.Colorless);
      Update(() => MultiCount != pool.Multi, () => MultiCount = pool.Multi);
    }

    private static void Update(Func<bool> condition, Action update)
    {
      if (condition()) update();
    }
  }
}