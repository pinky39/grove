namespace Grove.Modifiers
{
  public class AddStaticAbility : Modifier, ICardModifier
  {
    private readonly StaticAbility _staticAbility;
    private StaticAbilities _abilities;
    private AddToList<StaticAbility> _modifier;
    private AddStaticAbility()
    {
    }

    public AddStaticAbility(StaticAbility staticAbility)
    {
      _staticAbility = staticAbility;
    }
    
    public override void Apply(StaticAbilities abilities)
    {
      _abilities = abilities;
      _modifier = new AddToList<StaticAbility>(_staticAbility);
      _modifier.Initialize(ChangeTracker);
      _staticAbility.Initialize(OwningCard, Game);

      abilities.AddModifier(_modifier);
    }
    protected override void Unapply()
    {
      _abilities.RemoveModifier(_modifier);
    }
  }
}