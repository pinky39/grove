namespace Grove.Effects
{
  public class AddManaToPool : Effect
  {
    private readonly DynParam<ManaAmount> _amount;
    private readonly ManaUsage _usage;

    private AddManaToPool() {}  

    public AddManaToPool(DynParam<ManaAmount> amount, ManaUsage usage = ManaUsage.Any)
    {
      _amount = amount;
      _usage = usage;

      RegisterDynamicParameters(amount);
    }

    protected override void ResolveEffect()
    {
      Controller.AddManaToManaPool(_amount.Value, _usage);
    }
  }
}