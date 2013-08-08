namespace Grove.UserInterface.Shell
{
  using System.Windows;
  using Caliburn.Micro;
  using Gameplay;
  using Infrastructure;
  using MessageBox;
  using Messages;

  public class Shell : IShell, IHaveDisplayName
  {
    private readonly CardDatabase _cardDatabase;
    private readonly CardFactory _cardFactory;
    private readonly Publisher _publisher = new Publisher().Initialize();
    private InteractionState _interactionState = InteractionState.Disabled;

    public Shell(CardDatabase cardDatabase, CardFactory cardFactory)
    {
      _cardDatabase = cardDatabase;
      _cardFactory = cardFactory;
      DisplayName = "magicgrove";
    }

    public virtual object Screen { get; protected set; }
    public virtual object Dialog { get; protected set; }
    public string DisplayName { get; set; }

    public void CloseAllDialogs()
    {
      var dialogHost = Screen as IIsDialogHost;
      if (dialogHost == null)
        return;

      dialogHost.CloseAllDialogs();
    }

    public void ChangeScreen(object screen, bool blockUntilClosed = false, bool shouldClosePrevious = true)
    {
      if (Screen != null && shouldClosePrevious)
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


      var revert = ChangeMode(interactionState);

      if (dialogHost == null)
      {
        Dialog = dialog;
      }
      else
      {
        dialogHost.AddDialog(dialog, type);
      }

      ((IClosable) dialog).Closed += delegate
        {
          if (dialogHost == null)
          {
            Dialog = null;
          }
          else
          {
            dialogHost.RemoveDialog(dialog);
          }

          ChangeMode(revert);
        };
    }

    public MessageBoxResult ShowMessageBox(string message, MessageBoxButton buttons,
      DialogType type = DialogType.Large, string title = "", MessageBoxImage icon = MessageBoxImage.Question)
    {
      var messageBox = new ViewModel(message, title, buttons, type, icon);
      ShowModalDialog(messageBox, type, InteractionState.Disabled);
      return messageBox.Result;
    }

    public void ShowModalDialog(object dialog, DialogType type, InteractionState? interactionState = null)
    {
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