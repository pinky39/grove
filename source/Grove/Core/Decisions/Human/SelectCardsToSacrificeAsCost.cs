namespace Grove.Core.Controllers.Human
{
  using System.Windows;
  using Ui;
  using Ui.Shell;

  public class SelectCardsToSacrificeAsCost : Controllers.SelectCardsToSacrificeAsCost
  {
    public CardSelector CardSelector { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        message: QuestionText,
        buttons: MessageBoxButton.YesNo,
        type: DialogType.Small);

      if (result != MessageBoxResult.Yes)
        return;

      CardSelector.ExecuteQuery(this);
    }
  }
}