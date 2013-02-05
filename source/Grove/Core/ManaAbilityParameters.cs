namespace Grove.Core
{
  using System;
  using Mana;

  public class ManaAbilityParameters : ActivatedAbilityParameters
  {
    public Func<ManaAbility, Game, IManaAmount> GetManaAmount;
    public int Priority = ManaSourcePriorities.Land;

    public void ManaAmount(Func<ManaAbility, Game, IManaAmount> getManaAmount)
    {
      GetManaAmount = getManaAmount;
    }

    public void ManaAmount(IManaAmount manaAmount)
    {
      GetManaAmount = delegate { return manaAmount; };
    }    

    public ManaAbilityParameters()
    {
      UsesStack = false;
    }
  }
}