namespace Grove.Core.Controllers.Human
{
  using System.Windows;
  using Ui;
  using Ui.Shell;

  public class PayCounterCost : Controllers.PayCounterCost
  {
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        message: string.Format("{0}: Pay {1}?", Spell.Source.SourceCard, DoNotCounterCost),
        buttons: MessageBoxButton.YesNo,
        type: DialogType.Small);

      Result = result == MessageBoxResult.Yes;
    }
  }
}