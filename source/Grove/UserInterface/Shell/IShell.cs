namespace Grove.UserInterface.Shell
{
  using System.Windows;

  public interface IShell
  {
    void ChangeScreen(object screen, bool blockUntilClosed = false, bool shouldClosePrevious = true);
    void ShowDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null);

    MessageBoxResult ShowMessageBox(string message, MessageBoxButton buttons, DialogType type = DialogType.Large,
      string title = "", MessageBoxImage icon = MessageBoxImage.Question);

    void ShowModalDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null);

    bool HasFocus(object dialog);
    void CloseAllDialogs();
  }
}