namespace Grove.Core.Controllers.Human
{
  using System;
  using Results;
  using Targeting;
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

      foreach (var selector in TargetSelectors.Effect())
      {        
        if (NoValidTargets(selector))
          continue;
        
        var dialog = DialogFactory.Create(selector, canCancel: false);
        Shell.ShowModalDialog(dialog, DialogType.Small, SelectionMode.SelectTarget);

        targets.AddEffect(dialog.Selection[0]);
      }

      Result = new ChosenTargets(targets);
    }

    private bool NoValidTargets(TargetSelector selector)
    {
      foreach (var target in Game.Players.GetTargets())
      {
        if (selector.IsValid(target))
          return false;
      }

      return true;
    }
  }
}