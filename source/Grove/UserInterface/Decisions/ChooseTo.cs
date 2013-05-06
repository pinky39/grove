namespace Grove.UserInterface.Decisions
{
  using System.Windows;
  using Shell;

  public class ChooseTo : Gameplay.Decisions.ChooseTo
  {
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        message: Text,
        type: DialogType.Small,
        buttons: MessageBoxButton.YesNo);

      Result = result == MessageBoxResult.Yes;
    }
  }
}