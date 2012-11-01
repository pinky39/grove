namespace Grove.Core.Controllers.Human
{
  using System.Linq;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;
  using Zones;

  public class ReturnCardsFromGraveyardToBattlefield : Controllers.ReturnCardsFromGraveyardToBattlefield
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
          isValid: target => target.Zone == Zone.Graveyard && Filter(target) && target.Controller == Controller),
        canCancel: false
        );

      Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);
      Result = dialog.Selection.ToList();
    }
  }
}