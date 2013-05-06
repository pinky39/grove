namespace Grove.UserInterface.Decisions
{
  using DamageOrder;
  using Gameplay.Decisions.Results;
  using Shell;

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