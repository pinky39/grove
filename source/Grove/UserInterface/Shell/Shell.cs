namespace Grove.UserInterface.Shell
{
  using System.Threading.Tasks;
  using System.Windows;
  using Caliburn.Micro;
  using Gameplay.Tournaments;
  using Infrastructure;
  using MessageBox;
  using Messages;

  public class Shell : IShell, IHaveDisplayName
  {
    private readonly PreConstructedLimitedDecks _preConstructedLimitedDecks;
    private readonly Publisher _publisher = new Publisher().Initialize();
    private InteractionState _interactionState = InteractionState.Disabled;

    public Shell(PreConstructedLimitedDecks preConstructedLimitedDecks)
    {
      _preConstructedLimitedDecks = preConstructedLimitedDecks;
      DisplayName = "magicgrove";
      LoadResources();
    }

    public virtual object Screen { get; protected set; }
    public virtual bool HasLoaded { get; protected set; }
    public string DisplayName { get; set; }

    public void CloseAllDialogs()
    {
      var dialogHost = Screen as IIsDialogHost;
      if (dialogHost == null)
        return;

      dialogHost.CloseAllDialogs();
    }

    public void ChangeScreen(object screen, bool blockUntilClosed = false)
    {
      if (Screen != null)
      {
        Screen.Close();
      }

      Screen = screen;

      if (blockUntilClosed == false)
        return;

      var blocker = new ThreadBlocker();
      blocker.BlockUntilCompleted(() => { ((IClosable) screen).Closed += delegate { blocker.Completed(); }; });
    }

    public void Publish<T>(T message)
    {
      _publisher.Publish(message);
    }

    public void Subscribe(object instance)
    {
      _publisher.Subscribe(instance);
    }

    public void Unsubscribe(object instance)
    {
      _publisher.Unsubscribe(instance);
    }

    public void ShowDialog(object dialog, DialogType type, InteractionState? interactionState = null)
    {
      var dialogHost = Screen as IIsDialogHost;
      if (dialogHost == null)
        return;

      var revert = ChangeMode(interactionState);

      dialogHost.AddDialog(dialog, type);
      ((IClosable) dialog).Closed += delegate
        {
          dialogHost.RemoveDialog(dialog);
          ChangeMode(revert);
        };
    }

    public MessageBoxResult ShowMessageBox(string message, MessageBoxButton buttons,
      DialogType type = DialogType.Large, string title = "")
    {
      var messageBox = new ViewModel(message, title, buttons, type);
      ShowModalDialog(messageBox, type, InteractionState.Disabled);
      return messageBox.Result;
    }

    public void ShowModalDialog(object dialog, DialogType type, InteractionState? interactionState = null)
    {
      var dialogHost = Screen as IIsDialogHost;
      if (dialogHost == null)
        return;

      var blocker = new ThreadBlocker();

      blocker.BlockUntilCompleted(() =>
        {
          ShowDialog(dialog, type, interactionState);
          ((IClosable) dialog).Closed += delegate { blocker.Completed(); };
        });
    }

    public bool HasFocus(object dialog)
    {
      var dialogHost = Screen as IIsDialogHost;
      if (dialogHost == null)
        return false;

      return dialogHost.HasFocus(dialog);
    }

    private void LoadResources()
    {
      Task.Factory.StartNew(() =>
        {
          MediaLibrary.LoadResources();
          _preConstructedLimitedDecks.Load();
          HasLoaded = true;
        });
    }

    private InteractionState? ChangeMode(InteractionState? interactionState)
    {
      if (interactionState.HasValue)
      {
        var revert = _interactionState;
        _interactionState = interactionState.Value;

        Publish(new UiInteractionChanged
          {
            State = interactionState.Value
          });

        return revert;
      }

      return null;
    }
  }
}