namespace Grove.Core.Controllers.Human
{
  using Results;
  using Ui.CombatDamage;
  using Ui.Shell;

  public class AssignCombatDamage : Controllers.AssignCombatDamage
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = new DamageDistribution();
      
      var dialog = DialogFactory.Create(Attacker, result);
      Shell.ShowModalDialog(dialog);

      Result = result;
    }
  }
}