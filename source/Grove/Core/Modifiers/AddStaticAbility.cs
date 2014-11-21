namespace Grove.Modifiers
{
  public class AddStaticAbility : Modifier, ICardModifier
  {
    private readonly Static _staticAbility;
    private SimpleAbilities _abilities;
    private AddToList<Static> _modifier;

    private AddStaticAbility() {}

    public AddStaticAbility(Static staticAbility)
    {
      _staticAbility = staticAbility;
    }

    public override void Apply(SimpleAbilities abilities)
    {
      _modifier = new AddToList<Static>(_staticAbility);
      _modifier.Initialize(ChangeTracker);
      _abilities = abilities;
      _abilities.AddModifier(_modifier);
    }

    protected override void Unapply()
    {
      _abilities.RemoveModifier(_modifier);
    }
  }
}