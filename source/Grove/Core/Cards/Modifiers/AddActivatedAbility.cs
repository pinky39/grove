namespace Grove.Core.Cards.Modifiers
{
  using Targeting;

  public class AddActivatedAbility : Modifier
  {
    private ActivatedAbilities _abilities;
    private ActivatedAbility _ability;

    public IActivatedAbilityFactory Ability { get; set; }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      _ability = Ability.Create(Target.Card(), Game);
      _abilities.Add(_ability);
    }

    protected override void Unapply()
    {
      _abilities.Remove(_ability);
    }
  }
}