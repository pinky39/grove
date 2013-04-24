namespace Grove.Ui.Decisions
{
  using System.Windows;
  using Grove.Ui.Shell;

  public class ChooseToUntap : Gameplay.Decisions.ChooseToUntap
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