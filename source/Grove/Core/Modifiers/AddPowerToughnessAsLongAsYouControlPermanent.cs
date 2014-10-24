namespace Grove.Modifiers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class AddPowerToughnessAsLongAsYouControlPermanent : Modifier, IReceive<ZoneChangedEvent>,
    IReceive<PermanentModifiedEvent>, ICardModifier
  {
    private readonly int _power;
    private readonly IntegerModifier _powerModifier;
    private readonly Func<Card, bool> _selector;
    private readonly int _toughness;
    private readonly IntegerModifier _toughnessModifier;
    private Strenght _strenght;

    protected AddPowerToughnessAsLongAsYouControlPermanent() {}

    public AddPowerToughnessAsLongAsYouControlPermanent(
      int power,
      int toughness,
      Func<Card, bool> selector)
    {
      _power = power;
      _toughness = toughness;
      _selector = selector;

      _toughnessModifier = new IntegerIncrement(0);
      _powerModifier = new IntegerIncrement(0);
    }

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;

      _strenght.AddPowerModifier(_powerModifier);
      _strenght.AddToughnessModifier(_toughnessModifier);
    }

    public void Receive(PermanentModifiedEvent message)
    {
      Update();
    }

    public void Receive(ZoneChangedEvent message)
    {
      Update();
    }

    public bool CheckCondition()
    {
      return OwningCard.Controller.Battlefield.Any(_selector);
    }

    protected override void Initialize()
    {
      _toughnessModifier.Initialize(ChangeTracker);
      _powerModifier.Initialize(ChangeTracker);

      Update();
    }

    protected override void Unapply()
    {
      _strenght.RemovePowerModifier(_powerModifier);
      _strenght.RemoveToughnessModifier(_toughnessModifier);
    }

    private void Update()
    {
      if (CheckCondition())
      {
        _powerModifier.Value = _power;
        _toughnessModifier.Value = _toughness;
      }
      else
      {
        _powerModifier.Value = 0;
        _toughnessModifier.Value = 0;
      }
    }
  }
}