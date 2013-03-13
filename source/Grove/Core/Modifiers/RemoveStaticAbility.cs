namespace Grove.Core.Modifiers
{
  using Infrastructure;

  public class RemoveStaticAbility : Modifier
  {
    private readonly Static _staticAbility;
    private StaticAbilities _abilities;
    private readonly Trackable<bool> _removed = new Trackable<bool>();

    public RemoveStaticAbility(Static staticAbility)
    {
      _staticAbility = staticAbility;
    }

    protected override void Initialize()
    {
      _removed.Initialize(ChangeTracker);
    }

    public override void Apply(StaticAbilities abilities)
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