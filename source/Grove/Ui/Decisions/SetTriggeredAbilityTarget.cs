namespace Grove.Ui.Decisions
{
  using Grove.Gameplay.Decisions.Results;
  using Grove.Gameplay.Targeting;
  using Grove.Ui.SelectTarget;
  using Grove.Ui.Shell;
  using System.Linq;

  public class SetTriggeredAbilityTarget : Gameplay.Decisions.SetTriggeredAbilityTarget
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