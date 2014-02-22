namespace Grove.Gameplay.Modifiers
{
  public class AddStaticAbility : Modifier, ICardModifier
  {
    private readonly Static _staticAbility;
    private SimpleAbilities _abilities;

    private AddStaticAbility() {}

    public AddStaticAbility(Static staticAbility)
    {
      _staticAbility = staticAbility;
    }

    public override void Apply(SimpleAbilities abilities)
    {
      _abilities = abilities;
      _abilities.Add(_staticAbility);
    }

    protected override void Unapply()
    {
      _abilities.Remove(_staticAbility);
    }
  }
}