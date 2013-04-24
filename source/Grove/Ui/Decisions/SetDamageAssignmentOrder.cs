namespace Grove.Core.Decisions.Human
{
  using Grove.Ui.DamageOrder;
  using Grove.Ui.Shell;
  using Results;

  public class SetDamageAssignmentOrder : Decisions.SetDamageAssignmentOrder
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