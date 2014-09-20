namespace Grove.Modifiers
{
  using Grove.Infrastructure;

  public class RemoveStaticAbility : Modifier, ICardModifier
  {
    private readonly Trackable<bool> _removed = new Trackable<bool>();
    private readonly Static _staticAbility;
    private SimpleAbilities _abilities;

    private RemoveStaticAbility() {}

    public RemoveStaticAbility(Static staticAbility)
    {
      _staticAbility = staticAbility;
    }

    protected override void Initialize()
    {
      _removed.Initialize(ChangeTracker);
    }

    public override void Apply(SimpleAbilities abilities)
    {
      _abilities = abilities;
      _removed.Value = _abilities.Remove(_staticAbility);
    }

    protected override void Unapply()
    {
      if (_removed)
      {
        _abilities.Add(_staticAbility);
      }
    }
  }
}