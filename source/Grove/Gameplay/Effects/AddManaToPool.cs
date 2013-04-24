namespace Grove.Core.Effects
{
  using Mana;

  public class AddManaToPool : Effect
  {
    private readonly DynParam<IManaAmount> _amount;
    private readonly ManaUsage _usage;

    private AddManaToPool() {}

    public AddManaToPool(IManaAmount amount, ManaUsage usage = ManaUsage.Any)
    {
      _usage = usage;
      _amount = new DynParam<IManaAmount>(amount);


      RegisterDynamicParameters(_amount);
    }

    public AddManaToPool(DynParam<IManaAmount> amount, ManaUsage usage = ManaUsage.Any)
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