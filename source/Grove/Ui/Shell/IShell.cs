namespace Grove.Ui.Shell
{
  using System.Windows;

  public interface IShell
  {
    void ChangeScreen(IIsDialogHost screen);
    void ShowDialog(object dialog, DialogType type = DialogType.Large, SelectionMode? selectionMode = null);

    MessageBoxResult ShowMessageBox(string message, MessageBoxButton buttons, DialogType type = DialogType.Large,
                                    string title = "");

    void ShowModalDialog(object dialog, DialogType type = DialogType.Large, SelectionMode? selectionMode = null);
    void ShowNotification(string message);

    bool HasFocus(object dialog);
  }
}