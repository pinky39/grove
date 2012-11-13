namespace Grove.Core.Cards.Effects
{
  using Mana;

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