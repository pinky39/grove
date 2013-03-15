namespace Grove.Core.Decisions.Human
{
  using System;
  using System.Linq;
  using Targeting;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;
  using Zones;

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
          Message = String.Format("Select {0} card(s) to discard.", Count),
          TargetSpec = p => Filter(p.Target.Card()),
          ZoneSpec = p => p.ZoneOwner == CardsOwner && p.Zone == Zone.Hand
        };

      var targetValidator = new TargetValidator(parameters);
      targetValidator.Initialize(Game, Controller);

      var dialog = DialogFactory.Create(new SelectTargetParameters
        {
          CanCancel = false,
          Validator = targetValidator
        });

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      Result = dialog.Selection.ToList();
    }
  }
}