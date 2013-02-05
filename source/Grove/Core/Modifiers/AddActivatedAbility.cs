namespace Grove.Core.Modifiers
{
  using System;
  using Targeting;

  public class AddActivatedAbility : Modifier
  {
    private ActivatedAbilities _abilities;
    private ActivatedAbility _ability;
    private readonly Func<ActivatedAbility> _abilityFactory;

    private AddActivatedAbility() {}

    public AddActivatedAbility(Func<ActivatedAbility> abilityFactory)
    {
      _abilityFactory = abilityFactory;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      _ability = _abilityFactory();
      
      _ability.Initialize(Target.Card(), Game);        
      _abilities.Add(_ability);
    }

    protected override void Unapply()
    {
      _abilities.Remove(_ability);
    }
  }
}