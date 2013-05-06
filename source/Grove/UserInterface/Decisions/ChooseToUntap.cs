namespace Grove.UserInterface.Decisions
{
  using System.Windows;
  using Shell;

  public class ChooseToUntap : Gameplay.Decisions.ChooseToUntap
  {
    public ChooseToUntap()
    {
      Result = false;
    }

    public IShell Shell { get; set; }

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