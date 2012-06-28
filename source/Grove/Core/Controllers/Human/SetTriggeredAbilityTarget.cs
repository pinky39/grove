namespace Grove.Core.Controllers.Human
{
  using Results;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class SetTriggeredAbilityTarget : Controllers.SetTriggeredAbilityTarget
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var targets = new Targets();

      foreach (var selector in TargetSelectors)
      {
        var dialog = DialogFactory.Create(selector.Value, canCancel: false);
        Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);

        targets[selector.Key] = dialog.Selection[0];
      }
                  
      Result = new ChosenTargets(targets);
    }
  }
}