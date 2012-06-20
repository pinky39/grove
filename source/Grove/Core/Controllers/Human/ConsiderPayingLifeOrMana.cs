namespace Grove.Core.Controllers.Human
{
  using System;
  using System.Windows;
  using Ui;
  using Ui.Shell;

  public class ConsiderPayingLifeOrMana : Controllers.ConsiderPayingLifeOrMana
  {
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var message = Message ?? (Life != null
        ? String.Format("Pay {0}?", Life)
        : String.Format("Pay {0} mana?", Mana.Converted));

      var result = Shell.ShowMessageBox(
        message: message,
        buttons: MessageBoxButton.YesNo,
        type: DialogType.Small);

      Result = result == MessageBoxResult.Yes;
    }
  }
}