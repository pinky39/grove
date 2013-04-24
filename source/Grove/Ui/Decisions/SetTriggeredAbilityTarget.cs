namespace Grove.Core.Decisions.Human
{
  using Results;
  using Targeting;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;
  using System.Linq;

  public class SetTriggeredAbilityTarget : Decisions.SetTriggeredAbilityTarget
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var targets = new Targets();

      foreach (var validator in TargetSelector.Effect)
      {
        if (NoValidTargets(validator))
          continue;

        var selectTargetParameters = new SelectTargetParameters
        {          
          Validator = validator,
          CanCancel = false,
          TriggerMessage = TriggerMessage
        };

        var dialog = DialogFactory.Create(selectTargetParameters);
        Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

        foreach (var target in dialog.Selection)
        {
          targets.AddEffect(target);
        }
      }

      Result = new ChosenTargets(targets);
    }

    private bool NoValidTargets(TargetValidator validator)
    {
      foreach (var target in Players.SelectMany(x => x.GetTargets(validator.IsZoneValid)))
      {
        if (validator.IsTargetValid(target, TriggerMessage))
          return false;
      }

      return true;
    }
  }
}