namespace Grove.Ui.Decisions
{
  using Grove.Gameplay.Decisions.Results;
  using Grove.Ui.DamageOrder;
  using Grove.Ui.Shell;

  public class SetDamageAssignmentOrder : Gameplay.Decisions.SetDamageAssignmentOrder
  {
    public ViewModel.IFactory DialogFactory { get; set; }
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = new DamageAssignmentOrder();

      var dialog = DialogFactory.Create(Attacker, result);
      Shell.ShowModalDialog(dialog);

      Result = result;
    }
  }
}