namespace Grove.Core.Controllers.Human
{
  using Results;
  using Targeting;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class CardSelector
  {
    public ViewModel.IFactory TargetDialog { get; set; }
    public IShell Shell { get; set; }

    public void ExecuteQuery(SelectCards selectCards)
    {
      var chosenCards = new ChosenCards();

      var dialog = TargetDialog.Create(
        new UiTargetValidator(
          minTargetCount: selectCards.MinCount,
          maxTargetCount: selectCards.MaxCount,
          text: selectCards.Text,
          isValid: card => selectCards.Controller == card.Controller && selectCards.Validator(card)),
        canCancel: false
        );

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

      foreach (var target in dialog.Selection)
      {
        chosenCards.Add(target.Card());
      }

      selectCards.Result = chosenCards;
    }
  }
}