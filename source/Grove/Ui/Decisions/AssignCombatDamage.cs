namespace Grove.Ui.Decisions
{
  using CombatDamage;
  using Gameplay.Decisions.Results;
  using Shell;

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