namespace Grove.UserInterface.Shell
{
  using System.Windows;

  public interface IShell
  {
    void ChangeScreen(object screen, bool blockUntilClosed = false);
    void ShowDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null);

    MessageBoxResult ShowMessageBox(string message, MessageBoxButton buttons, DialogType type = DialogType.Large,
      string title = "");

    void ShowModalDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null);

    bool HasFocus(object dialog);
    void CloseAllDialogs();

    void Publish<T>(T message);

    void Subscribe(object instance);
    void Unsubscribe(object instance);
  }
}