namespace Grove.Core.Controllers.Human
{
  using System.Windows;
  using Ui;
  using Ui.Shell;

  public class PayOr : Controllers.PayOr
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