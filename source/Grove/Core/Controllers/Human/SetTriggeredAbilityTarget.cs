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
      var dialog = DialogFactory.Create(TargetSelector, canCancel: false);
      Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);

      Result = new ChosenTarget(dialog.Selection[0]);
    }
  }
}