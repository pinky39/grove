namespace Grove.Ui.ManaPool
{
  using System;
  using System.Threading;
  using Core;

  public class ViewModel : IDisposable
  {
    private readonly Timer _timer;
    private readonly Player _human;

    public ViewModel(Game game)
    {
      _human = game.Players.Human;

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
    
    private void Update()
    {
      Update(() => WhiteCount != _human.ManaPool.WhiteCount, () => WhiteCount = _human.ManaPool.WhiteCount);
      Update(() => BlueCount != _human.ManaPool.BlueCount, () => BlueCount = _human.ManaPool.BlueCount);
      Update(() => BlackCount != _human.ManaPool.BlackCount, () => BlackCount = _human.ManaPool.BlackCount);
      Update(() => RedCount != _human.ManaPool.RedCount, () => RedCount = _human.ManaPool.RedCount);
      Update(() => GreenCount != _human.ManaPool.GreenCount, () => GreenCount = _human.ManaPool.GreenCount);
      Update(() => ColorlessCount != _human.ManaPool.ColorlessCount, () => ColorlessCount = _human.ManaPool.ColorlessCount);
      Update(() => MultiCount != _human.ManaPool.MultiCount, () => MultiCount = _human.ManaPool.MultiCount);
    }

    private static void Update(Func<bool> condition, Action update)
    {
      if (condition()) update();
    }
    
    public void Dispose()
    {
      _timer.Dispose();
    }
  }
}