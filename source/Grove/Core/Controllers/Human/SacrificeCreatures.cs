namespace Grove.Core.Controllers.Human
{
  using System.Linq;
  using Ui;
  using Ui.SelectTarget;
  using Ui.Shell;

  public class SacrificeCreatures : Controllers.SacrificeCreatures
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }
        
    protected override void ExecuteQuery()
    {
      var dialog = DialogFactory.Create(
        new UiTargetSelector(
          minTargetCount: Count,
          maxTargetCount: Count,
          text: string.Format("Select {0} creature(s) to sacrifice.", Count),
          isValid: target => target.Is().Creature), 
          canCancel: false
        );
            
      Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);

      Result = dialog.Selection.ToList();            
    }
  }
}