namespace Grove.Core.Controllers.Human
{
  using System.Linq;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class ReturnPermanentsToHand : Controllers.ReturnPermanentsToHand
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var dialog = DialogFactory.Create(
        new UiTargetValidator(
          minTargetCount: Count,
          maxTargetCount: Count,
          text: string.Format(Text, Count),
          isValid: target => target.IsPermanent && Filter(target) && target.Controller == Controller),
        canCancel: false
        );

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      Result = dialog.Selection.ToList();
    }
  }
}