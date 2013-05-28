namespace Grove.Gameplay.Modifiers
{
  using System;
  using Abilities;
  using Targeting;

  [Serializable]
  public class AddActivatedAbility : Modifier
  {
    private readonly ActivatedAbility _activatedAbility;
    private ActivatedAbilities _abilities;

    private AddActivatedAbility() {}

    public AddActivatedAbility(ActivatedAbility activatedAbility)
    {
      _activatedAbility = activatedAbility;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      _activatedAbility.Initialize(Target.Card(), Game);
      _abilities.Add(_activatedAbility);
    }

    protected override void Unapply()
    {
      _abilities.Remove(_activatedAbility);
    }
  }
}