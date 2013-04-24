namespace Grove.Ui.Shell
{
  using System.Windows;
  using Caliburn.Micro;
  using Core;
  using Gameplay;
  using Infrastructure;
  using MessageBox;

  public class Shell : IShell, IHaveDisplayName
  {
    private readonly Match _match;
    private InteractionState _interactionState = InteractionState.Disabled;    

    public Shell(Match match)
    {
      _match = match;
      _match.SetShell(this);

      DisplayName = "magicgrove";
    }

    public virtual IIsDialogHost Screen { get; protected set; }
    public string DisplayName { get; set; }       

    public void CloseAllDialogs()
    {
      Screen.CloseAllDialogs();
    }

    public void ChangeScreen(IIsDialogHost screen)
    {
      if (Screen != null)
      {
        Screen.Close();
      }

      Screen = screen;
    }

    public void ShowDialog(object dialog, DialogType type, InteractionState? interactionState = null)
    {
      var revert = ChangeMode(interactionState);

      Screen.AddDialog(dialog, type);
      ((IClosable) dialog).Closed += delegate
        {
          Screen.RemoveDialog(dialog);

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
      var blocker = new ThreadBlocker();

      blocker.BlockUntilCompleted(() =>
        {
          ShowDialog(dialog, type, interactionState);
          ((IClosable) dialog).Closed += delegate { blocker.Completed(); };
        });

      Screen.RemoveDialog(dialog);
    }

    public bool HasFocus(object dialog)
    {
      return Screen.HasFocus(dialog);
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