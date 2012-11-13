namespace Grove.Core.Decisions.Human
{
  using System.Windows;
  using Grove.Ui;
  using Grove.Ui.Shell;

  public class PayOr : Decisions.PayOr
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