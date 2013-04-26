namespace Grove.Ui.Decisions
{
  using System.Windows;
  using Shell;

  public class TakeMulligan : Gameplay.Decisions.TakeMulligan
  {
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        title: "Mulligan",
        message: "Do you want to improve your hand by taking a mulligan?",
        buttons: MessageBoxButton.YesNo);

      Result = result == MessageBoxResult.Yes;
    }
  }
}