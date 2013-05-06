namespace Grove.UserInterface.Decisions
{
  using System.Windows;
  using Shell;

  public class PayOr : Gameplay.Decisions.PayOr
  {
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        message: Text,
        buttons: MessageBoxButton.YesNo,
        type: DialogType.Small);

      Result = result == MessageBoxResult.Yes;
    }
  }
}