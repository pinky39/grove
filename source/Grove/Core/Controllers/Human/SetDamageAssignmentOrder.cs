namespace Grove.Core.Controllers.Human
{
  using Results;
  using Ui.DamageOrder;
  using Ui.Shell;

  public class SetDamageAssignmentOrder : Controllers.SetDamageAssignmentOrder
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