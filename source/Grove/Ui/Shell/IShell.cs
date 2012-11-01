namespace Grove.Ui.Shell
{
  using System.Windows;

  public interface IShell
  {
    void ChangeScreen(IIsDialogHost screen);
    void ShowDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null);

    MessageBoxResult ShowMessageBox(string message, MessageBoxButton buttons, DialogType type = DialogType.Large,
                                    string title = "");

    void ShowModalDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null);    

    bool HasFocus(object dialog);
    void CloseAllDialogs();
  }
}