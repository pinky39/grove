namespace Grove.UserInterface.Decisions
{
  using System.Linq;
  using Gameplay;
  using Gameplay.Decisions.Results;
  using Gameplay.Targeting;
  using Messages;
  using SelectTarget;
  using Shell;

  public class DeclareBlockers : Gameplay.Decisions.DeclareBlockers
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = new ChosenBlockers();

      while (true)
      {                        
        var blockerTarget = new TargetValidatorParameters
          {
            MinCount = result.IsValid() ? 0 : 1, 
            MaxCount = 1, Message = "Select a blocker."
          }
          .Is.Card(c => c.CanBlock() && c.Controller == Controller)
          .On.Battlefield();

        blockerTarget.MustBeTargetable = false;

        var blockerValidator = new TargetValidator(blockerTarget);
        blockerValidator.Initialize(Game, Controller);

        var selectBlocker = DialogFactory.Create(new SelectTargetParameters
          {
            Validator = blockerValidator,
            CanCancel = false,
            Instructions = result.IsValid() ? "(Press Spacebar to finish.)" : "(Additional blockers required.)"
          });

        Shell.ShowModalDialog(selectBlocker, DialogType.Small, InteractionState.SelectTarget);

        if (selectBlocker.Selection.Count() == 0)
        {
          break;
        }

        var blocker = (Card) selectBlocker.Selection[0];

        if (result.ContainsBlocker(blocker))
        {
          result.Remove(blocker);

          Shell.Publish(new BlockerUnselected
            {
              Blocker = blocker
            });

          continue;
        }

        var attackerTarget =
          new TargetValidatorParameters {MinCount = 1, MaxCount = 1, Message = "Select an attacker to block."}
            .Is.Card(c => c.IsAttacker && c.CanBeBlockedBy(blocker))
            .On.Battlefield();

        attackerTarget.MustBeTargetable = false;

        var attackerValidator = new TargetValidator(attackerTarget);
        attackerValidator.Initialize(Game, Controller);

        var selectAttacker = DialogFactory.Create(new SelectTargetParameters
          {
            Validator = attackerValidator,
            CanCancel = true,
            Instructions = "(Press Esc to cancel.)"
          });

        Shell.ShowModalDialog(selectAttacker, DialogType.Small, InteractionState.SelectTarget);

        if (selectAttacker.WasCanceled)
          continue;

        var attacker = (Card) selectAttacker.Selection[0];

        Shell.Publish(new BlockerSelected
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