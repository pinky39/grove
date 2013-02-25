namespace Grove.Core.Effects
{
  using System;
  using Mana;

  public class AddManaToPool : Effect
  {
    private readonly Func<Effect, IManaAmount> _amount;
    private readonly bool _useOnlyForAbilities;

    private AddManaToPool() {}

    public AddManaToPool(IManaAmount amount, bool useOnlyForAbilities = false)
      : this(delegate { return amount; }, useOnlyForAbilities) {}


    public AddManaToPool(Func<Effect, IManaAmount> amount, bool useOnlyForAbilities = false)
    {
      _amount = amount;
      _useOnlyForAbilities = useOnlyForAbilities;
    }

    protected override void ResolveEffect()
    {
      Controller.AddManaToManaPool(_amount(this), _useOnlyForAbilities);
    }
  }
}