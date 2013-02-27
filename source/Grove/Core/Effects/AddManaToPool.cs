namespace Grove.Core.Effects
{
  using Mana;

  public class AddManaToPool : Effect
  {
    private readonly DynParam<IManaAmount> _amount;
    private readonly bool _useOnlyForAbilities;

    private AddManaToPool() {}

    public AddManaToPool(IManaAmount amount, bool useOnlyForAbilities = false)
    {
      _amount = new DynParam<IManaAmount>(amount);
      _useOnlyForAbilities = useOnlyForAbilities;

      RegisterDynamicParameters(_amount);
    }

    public AddManaToPool(DynParam<IManaAmount> amount, bool useOnlyForAbilities = false)
    {
      _amount = amount;
      _useOnlyForAbilities = useOnlyForAbilities;

      RegisterDynamicParameters(amount);
    }

    protected override void ResolveEffect()
    {
      Controller.AddManaToManaPool(_amount.Value, _useOnlyForAbilities);
    }
  }
}