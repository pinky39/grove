namespace Grove.Core.Decisions.Human
{
  using Grove.Core.Targeting;
  using Grove.Ui;
  using Grove.Ui.SelectTarget;
  using Grove.Ui.Shell;
  using Results;

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

        var dialog = DialogFactory.Create(validator, canCancel: false);
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
      foreach (var target in Game.Players.GetTargets())
      {
        if (validator.IsValid(target))
          return false;
      }

      return true;
    }
  }
}