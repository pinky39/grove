namespace Grove.Ui.ManaPool
{
  using System;
  using System.Threading;
  using Gameplay;
  using Gameplay.Player;

  public class ViewModel : IDisposable
  {
    private readonly Player _human;
    private readonly Timer _timer;

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

    public void Dispose()
    {
      _timer.Dispose();
    }

    private void Update()
    {
      var pool = _human.ManaPool;

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