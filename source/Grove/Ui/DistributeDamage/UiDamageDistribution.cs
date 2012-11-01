namespace Grove.Ui.DistributeDamage
{
  using System.Collections.Generic;
  using Core.Targeting;
  using Shell;

  public class UiDamageDistribution : IDamageDistributor
  {
    private readonly ViewModel.IFactory _distributeDamageDialog;
    private readonly IShell _shell;

    public UiDamageDistribution(IShell shell, ViewModel.IFactory distributeDamageDialog)
    {
      _shell = shell;
      _distributeDamageDialog = distributeDamageDialog;
    }

    public IList<int> DistributeDamage(IList<ITarget> targets, int damage)
    {
      if (targets.Count == 1)
      {
        return new[] {damage};
      }
      
      var dialog = _distributeDamageDialog.Create(targets, damage);
      _shell.ShowModalDialog(dialog, DialogType.Large, InteractionState.Disabled);

      return dialog.Distribution;
    }
  }
}