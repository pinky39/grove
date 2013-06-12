namespace Grove.Gameplay.Modifiers
{
  using Abilities;
  using Infrastructure;

  public class AddActivatedAbility : Modifier, ICardModifier
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
      _activatedAbility.Initialize(OwningCard, Game);
      _abilities.Add(_activatedAbility);
    }

    protected override void Unapply()
    {
      _abilities.Remove(_activatedAbility);
    }
  }
}