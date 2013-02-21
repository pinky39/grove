namespace Grove.Core.Decisions.Human
{
  using System.Linq;
  using Results;
  using Targeting;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class DeclareBlockers : Decisions.DeclareBlockers
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = new ChosenBlockers();

      while (true)
      {
        var blockerDefinition = new TargetValidatorParameters {MinCount = 0, MaxCount = 1, Text = "Select a blocker."}
          .Is.Card(c => c.CanBlock() && c.Controller == Controller)
          .On.Battlefield();

        var selectBlocker = DialogFactory.Create(new SelectTargetParameters
          {
            Validator = new TargetValidator(blockerDefinition),
            CanCancel = false,
            Instructions = "(Press Spacebar to finish.)"
          });

        Shell.ShowModalDialog(selectBlocker, DialogType.Small, InteractionState.SelectTarget);

        if (selectBlocker.Selection.Count() == 0)
          break;

        var blocker = (Card) selectBlocker.Selection[0];

        if (result.ContainsBlocker(blocker))
        {
          result.Remove(blocker);

          Publish(new BlockerUnselected
            {
              Blocker = blocker
            });

          continue;
        }

        var attackerDefinition =
          new TargetValidatorParameters {MinCount = 1, MaxCount = 1, Text = "Select an attacker to block."}
            .Is.Card(c => c.IsAttacker && c.CanBeBlockedBy(blocker))
            .On.Battlefield();

        var selectAttacker = DialogFactory.Create(new SelectTargetParameters
          {
            Validator = new TargetValidator(attackerDefinition),
            CanCancel = true,
            Instructions = "(Press Esc to cancel.)"
          });

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