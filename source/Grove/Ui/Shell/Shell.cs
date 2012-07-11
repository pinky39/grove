namespace Grove.Ui.Shell
{
  using System.Windows;
  using Caliburn.Micro;
  using Core;
  using Infrastructure;
  using MessageBox;

  public class Shell : IShell, IHaveDisplayName
  {
    private readonly Match _match;
    private SelectionMode _selectionMode = SelectionMode.Disabled;

    public Shell(Match match)
    {
      _match = match;
      _match.Shell = this;

      DisplayName = "magicgrove";
    }

    public virtual IIsDialogHost Screen { get; protected set; }
    public string DisplayName { get; set; }

    public void ChangeScreen(IIsDialogHost screen)
    {
      if (Screen != null)
      {
        Screen.Close();
      }

      Screen = screen;
    }

    public void ShowDialog(object dialog, DialogType type, SelectionMode? selectionMode = null)
    {
      var revert = ChangeMode(selectionMode);

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
      ShowModalDialog(messageBox, type, SelectionMode.Disabled);
      return messageBox.Result;
    }

    public void ShowModalDialog(object dialog, DialogType type, SelectionMode? selectionMode = null)
    {
      var blocker = new ThreadBlocker();

      blocker.BlockUntilCompleted(() =>
        {
          ShowDialog(dialog, type, selectionMode);
          ((IClosable) dialog).Closed += delegate { blocker.Completed(); };
        });

      Screen.RemoveDialog(dialog);
    }

    public void ShowNotification(string message)
    {
      var dialog = new Notification.ViewModel(message);
      ShowDialog(dialog, DialogType.Notification);
    }

    public bool HasFocus(object dialog)
    {
      return Screen.HasFocus(dialog);
    }

    private SelectionMode? ChangeMode(SelectionMode? selectionMode)
    {
      if (selectionMode.HasValue)
      {
        var revert = _selectionMode;
        _selectionMode = selectionMode.Value;

        _match.Game.Publisher.Publish(new SelectionModeChanged
          {
            SelectionMode = selectionMode.Value
          });

        return revert;
      }

      return null;
    }
  }
}