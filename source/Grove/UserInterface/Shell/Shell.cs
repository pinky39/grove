namespace Grove.UserInterface.Shell
{
  using System.Threading.Tasks;
  using System.Windows;
  using Caliburn.Micro;
  using Gameplay;
  using Infrastructure;
  using MessageBox;

  public class Shell : IShell, IHaveDisplayName
  {
    private readonly Match _match;
    private InteractionState _interactionState = InteractionState.Disabled;

    public Shell(Match match, CardsDatabase cardsDatabase)
    {
      _match = match;
      _match.SetShell(this);

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

    public void ChangeScreen(object screen)
    {
      if (Screen != null)
      {
        Screen.Close();
      }

      Screen = screen;
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

      dialogHost.RemoveDialog(dialog);
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
          HasLoaded = true;
        });
    }

    private InteractionState? ChangeMode(InteractionState? interactionState)
    {
      if (interactionState.HasValue)
      {
        var revert = _interactionState;
        _interactionState = interactionState.Value;

        if (_match.InProgress)
        {
          _match.Game.Publish(new UiInteractionChanged
            {
              State = interactionState.Value
            });
        }

        return revert;
      }

      return null;
    }
  }
}