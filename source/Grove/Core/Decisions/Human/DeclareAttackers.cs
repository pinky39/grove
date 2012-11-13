namespace Grove.Core.Decisions.Human
{
  using System.Linq;
  using Grove.Core.Targeting;
  using Grove.Ui;
  using Grove.Ui.SelectTarget;
  using Grove.Ui.Shell;

  public class DeclareAttackers : Decisions.DeclareAttackers
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