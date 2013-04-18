namespace Grove.Core
{
  using System;
  using Mana;

  //public class ManaAbilityParameters : ActivatedAbilityParameters
  //{
  //  public Func<ManaAbility, Game, IManaAmount> GetManaAmount;
  //  public int Priority = ManaSourcePriorities.Land;

  //  public ManaAbilityParameters ManaAmount(Func<ManaAbility, Game, IManaAmount> getManaAmount)
  //  {
  //    GetManaAmount = getManaAmount;
  //    return this;
  //  }

  //  public ManaAbilityParameters ManaAmount(ManaUnit manaUnit)
  //  {
  //    return ManaAmount(manaUnit.ToAmount());
  //  }
    
  //  public ManaAbilityParameters ManaAmount(IManaAmount manaAmount)
  //  {
  //    GetManaAmount = delegate { return manaAmount; };
  //    return this;
  //  }    

  //  public ManaAbilityParameters()
  //  {
  //    UsesStack = false;
  //  }
  //}
}