namespace Grove.Core.Decisions.Human
{
  using System.Linq;
  using Grove.Ui;
  using Grove.Ui.SelectTarget;
  using Grove.Ui.Shell;
  using Results;

  public class DeclareBlockers : Decisions.DeclareBlockers
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }    

    protected override void ExecuteQuery()
    {
      var result = new ChosenBlockers();

      while (true)
      {
        var selectBlocker = DialogFactory.Create(
          new UiTargetValidator(
            minTargetCount: 0,
            maxTargetCount: 1,
            text: "Select a blocker",
            isValid: target => target.CanBlock() &&
              target.Controller == Controller
            ), canCancel: false, instructions: "(Press Spacebar to finish.)"
          );

        Shell.ShowModalDialog(selectBlocker, DialogType.Small, InteractionState.SelectTarget);

        if (selectBlocker.Selection.Count() == 0)
          break;

        var blocker = (Card) selectBlocker.Selection[0];

        if (result.ContainsBlocker(blocker))
        {
          result.Remove(blocker);

          Game.Publish(new BlockerUnselected
            {
              Blocker = blocker
            });

          continue;
        }

        var selectAttacker = DialogFactory.Create(
          new UiTargetValidator(
            minTargetCount: 1,
            maxTargetCount: 1,
            text: "Select an attacker to block",
            isValid: target => Game.Combat.IsAttacker(target) && target.CanBeBlockedBy(blocker)
            ),
          canCancel: true,
          instructions: "(Press Esc to cancel.)"
          );

        Shell.ShowModalDialog(selectAttacker, DialogType.Small, InteractionState.SelectTarget);

        if (selectAttacker.WasCanceled)
          continue;

        var attacker = (Card) selectAttacker.Selection[0];

        Game.Publish(new BlockerSelected
          {
            Blocker = blocker,
            Attacker = attacker
          });

        result.Add(blocker, attacker);
      }

      Result = result;
    }
  }
}