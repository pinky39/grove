namespace Grove.Core
{
  using System;
  using Mana;

  public class ManaAbilityParameters : ActivatedAbilityParameters
  {
    public Func<ManaAbility, Game, IManaAmount> ManaAmount;

    public ManaAbilityParameters()
    {
      UsesStack = false;
    }
  }
}