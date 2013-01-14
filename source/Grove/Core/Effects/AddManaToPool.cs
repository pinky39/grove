namespace Grove.Core.Effects
{
  using Grove.Core.Mana;

  public class AddManaToPool : Effect
  {
    public IManaAmount Amount;
    public bool UseOnlyForAbilities;

    protected override void ResolveEffect()
    {            
      Controller.AddManaToManaPool(Amount, UseOnlyForAbilities);
    }
  }
}