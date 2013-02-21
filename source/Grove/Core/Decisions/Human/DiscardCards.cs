namespace Grove.Core.Decisions.Human
{
  using System;
  using System.Linq;
  using Targeting;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class DiscardCards : Decisions.DiscardCards
  {
    public IShell Shell { get; set; }
    public ViewModel.IFactory DialogFactory { get; set; }

    protected override void ExecuteQuery()
    {
      var parameters = new TargetValidatorParameters
        {
          MinCount = Count,
          MaxCount = Count,
          Text = String.Format("Select {0} card(s) to discard", Count),
        }
        .Is.Card(c => Filter(c)).In.OwnersHand();

      var dialog = DialogFactory.Create(new SelectTargetParameters
        {
          CanCancel = false,
          Validator = new TargetValidator(parameters)
        });

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      Result = dialog.Selection.ToList();
    }
  }
}