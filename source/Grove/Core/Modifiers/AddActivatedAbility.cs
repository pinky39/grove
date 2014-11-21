namespace Grove.Modifiers
{
  public class AddActivatedAbility : Modifier, ICardModifier
  {
    private readonly ActivatedAbility _activatedAbility;
    private ActivatedAbilities _abilities;
    private AddToList<ActivatedAbility> _modifier;

    private AddActivatedAbility() {}

    public AddActivatedAbility(ActivatedAbility activatedAbility)
    {
      _activatedAbility = activatedAbility;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;
      _modifier = new AddToList<ActivatedAbility>(_activatedAbility);
      _modifier.Initialize(ChangeTracker);
      _activatedAbility.Initialize(OwningCard, Game);
      _abilities.AddModifier(_modifier);
    }

    protected override void Unapply()
    {
      _abilities.RemoveModifier(_modifier);
    }
  }
}