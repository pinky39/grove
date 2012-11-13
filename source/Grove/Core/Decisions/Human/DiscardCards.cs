namespace Grove.Core.Decisions.Human
{
  using System;
  using System.Linq;
  using Grove.Ui;
  using Grove.Ui.SelectTarget;
  using Grove.Ui.Shell;
  using Grove.Core.Zones;

  public class DiscardCards : Decisions.DiscardCards
  {
    public IShell Shell { get; set; }
    public ViewModel.IFactory DialogFactory { get; set; }

    protected override void ExecuteQuery()
    {
      var dialog = DialogFactory.Create(new UiTargetValidator(
        minTargetCount: Count,
        maxTargetCount: Count,
        text: String.Format("Select {0} card(s) to discard", Count),
        isValid: card => card.Zone == Zone.Hand && card.Controller == CardsOwner && Filter(card)
        ), canCancel: false);


      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

      Result = dialog.Selection.ToList();
    }
  }
}