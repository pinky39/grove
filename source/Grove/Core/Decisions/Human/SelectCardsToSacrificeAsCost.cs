namespace Grove.Core.Decisions.Human
{
  using System.Windows;
  using Grove.Ui;
  using Grove.Ui.Shell;

  public class SelectCardsToSacrificeAsCost : Decisions.SelectCardsToSacrificeAsCost
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