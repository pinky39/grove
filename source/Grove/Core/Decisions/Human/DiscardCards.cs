namespace Grove.Core.Controllers.Human
{
  using System;
  using System.Linq;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;
  using Zones;

  public class DiscardCards : Controllers.DiscardCards
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