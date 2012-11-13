namespace Grove.Core.Decisions.Human
{
  using System.Windows;
  using Grove.Ui;
  using Grove.Ui.Shell;

  public class ChooseToUntap : Decisions.ChooseToUntap
  {
    public IShell Shell { get; set; }

    public ChooseToUntap()
    {
      Result = false;
    }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        message: string.Format("Untap {0}?", Permanent),
        buttons: MessageBoxButton.YesNo,
        type: DialogType.Small);

      Result = result == MessageBoxResult.Yes;
    }
  }
}