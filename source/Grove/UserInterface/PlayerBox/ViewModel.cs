namespace Grove.UserInterface.PlayerBox
{
  using System;
  using System.Threading;
  using Gameplay;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase, IDisposable, IReceive<UiInteractionChanged>
  {
    private readonly Timer _timer;

    public ViewModel(Player player, Game game)
    {
      Game = game;
      Player = player;

      Update();

      _timer = new Timer(delegate { Update(); }, null,
        TimeSpan.FromMilliseconds(20),
        TimeSpan.FromMilliseconds(20));
    }

    public Player Player { get; private set; }

    public virtual int HandCount { get; protected set; }
    public virtual int LibraryCount { get; protected set; }
    public virtual int GraveyardCount { get; protected set; }
    public virtual int Life { get; protected set; }
    public virtual bool IsActive { get; protected set; }
    public virtual bool CanChangeSelection { get; protected set; }

    public void Dispose()
    {
      _timer.Dispose();
    }

    public void Receive(UiInteractionChanged message)
    {
      CanChangeSelection = message.State == InteractionState.SelectTarget;
    }

    private void Update()
    {
      Update(() => HandCount != Player.Hand.Count, () => HandCount = Player.Hand.Count);
      Update(() => LibraryCount != Player.Library.Count, () => LibraryCount = Player.Library.Count);
      Update(() => GraveyardCount != Player.Graveyard.Count, () => GraveyardCount = Player.Graveyard.Count);
      Update(() => Life != Player.Life, () => Life = Player.Life);
      Update(() => IsActive = Player.IsActive, () => IsActive = Player.IsActive);
    }

    private static void Update(Func<bool> condition, Action update)
    {
      if (condition()) update();
    }

    public void ChangeSelection()
    {
      Shell.Publish(
        new SelectionChanged {Selection = Player});
    }

    public interface IFactory
    {
      ViewModel Create(Player player);
      void Release(ViewModel viewModel);
    }
  }
}