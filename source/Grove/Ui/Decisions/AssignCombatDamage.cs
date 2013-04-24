namespace Grove.Ui.Decisions
{
  using Grove.Gameplay.Decisions.Results;
  using Grove.Ui.CombatDamage;
  using Grove.Ui.Shell;

  public class AssignCombatDamage : Gameplay.Decisions.AssignCombatDamage
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