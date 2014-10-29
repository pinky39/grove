namespace Grove.Modifiers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class AddStaticAbilityAsLongAsYouControlPermanent : Modifier, IReceive<ZoneChangedEvent>,
    IReceive<PermanentModifiedEvent>, ICardModifier
  {
    private readonly Trackable<bool> _isActive = new Trackable<bool>();
    private readonly Func<Card, bool> _selector;
    private readonly Static _staticAbility;
    private SimpleAbilities _abilities;

    private AddStaticAbilityAsLongAsYouControlPermanent() {}

    public AddStaticAbilityAsLongAsYouControlPermanent(Static staticAbility, Func<Card, bool> selector)
    {
      _staticAbility = staticAbility;
      _selector = selector;
    }

    public override void Apply(SimpleAbilities abilities)
    {
      _abilities = abilities;
    }

    public void Receive(PermanentModifiedEvent message)
    {
      Update();
    }

    public void Receive(ZoneChangedEvent message)
    {
      Update();
    }

    protected override void Unapply()
    {
      if (_isActive)
      {
        _abilities.Remove(_staticAbility);
      }
    }

    private void Update()
    {
      var permanenents = Players.Permanents()
        .Where(c => c.Controller == OwningCard.Controller);

      var shouldBeActive = permanenents.Any(_selector);
      if (shouldBeActive && !_isActive)
      {
        _abilities.Add(_staticAbility);
      }
      else if (_isActive && !shouldBeActive)
      {
        _abilities.Remove(_staticAbility);
      }
    }

    protected override void Initialize()
    {
      _isActive.Initialize(ChangeTracker);
    }
  }
}