namespace Grove.Core.Decisions.Human
{
  using Grove.Core.Targeting;
  using Grove.Ui;
  using Grove.Ui.SelectTarget;
  using Grove.Ui.Shell;
  using Results;

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
          isValid: card => selectCards.Controller == card.Controller && selectCards.Zone(card.Zone) && selectCards.Validator(card)),
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