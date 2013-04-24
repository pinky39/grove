namespace Grove.Core.Modifiers
{
  public class AddStaticAbility : Modifier
  {
    private readonly Static _staticAbility;
    private StaticAbilities _abilities;

    private AddStaticAbility() {}

    public AddStaticAbility(Static staticAbility)
    {
      _staticAbility = staticAbility;
    }

    public override void Apply(StaticAbilities abilities)
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