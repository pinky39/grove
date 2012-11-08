namespace Grove.Core.Controllers.Human
{
  using System.Linq;
  using Infrastructure;
  using Targeting;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class DeclareAttackers : Controllers.DeclareAttackers
  {
    public ViewModel.IFactory DialogFactory { get; set; }    
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var dialog = DialogFactory.Create(
        new UiTargetValidator(
          minTargetCount: 0,
          maxTargetCount: null,
          text: "Select attackers",
          isValid: target => target.CanAttackThisTurn && target.Controller == Controller
          ),
        canCancel: false,
        instructions: "(Press Spacebar when done.)",
        targetSelected: target => Game.Publish(
          new AttackerSelected
            {
              Attacker = target.Card()
            }),
        targetUnselected: target => Game.Publish(
          new AttackerUnselected
            {
              Attacker = target.Card()
            }));

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

      Result = dialog.Selection.ToList();
    }
  }
}