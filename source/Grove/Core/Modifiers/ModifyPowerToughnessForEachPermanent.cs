namespace Grove.Modifiers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class ModifyPowerToughnessForEachPermanent : Modifier, IReceive<ZoneChangedEvent>, 
    IReceive<PermanentModifiedEvent>, ICardModifier
  {
    private readonly ControlledBy _controlledBy;
    private readonly CardSelector _filter;
    private readonly int? _modifyPower;
    private readonly int? _modifyToughness;
    private readonly IntegerModifier _powerModifier;
    private readonly IntegerModifier _toughnessModifier;
    private Strenght _strenght;

    protected ModifyPowerToughnessForEachPermanent() {}

    public ModifyPowerToughnessForEachPermanent(int? power, int? toughness, CardSelector filter,
      Func<IntegerModifier> modifier, ControlledBy controlledBy = ControlledBy.SpellOwner)
    {
      _modifyPower = power;
      _modifyToughness = toughness;
      _filter = filter;
      _controlledBy = controlledBy;

      _toughnessModifier = modifier();
      _powerModifier = modifier();
    }

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;

      if (_modifyPower.HasValue)
      {
        _strenght.AddPowerModifier(_powerModifier);
      }

      if (_modifyToughness.HasValue)
      {
        _strenght.AddToughnessModifier(_toughnessModifier);
      }
    }

    public void Receive(PermanentModifiedEvent message)
    {
      Update();
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (message.From == Zone.Battlefield)
      {
        Update();
      }

      else if (message.To == Zone.Battlefield)
      {
        if (!HasValidController(message.Card) || !_filter(message.Card, Ctx))
          return;

        IncreasePowerIfModified(_modifyPower);
        IncreaseToughnessIfModified(_modifyToughness);
      }
    }

    private bool HasValidController(Card card)
    {
      if (_controlledBy == ControlledBy.Any)
        return true;

      if (_controlledBy == ControlledBy.SpellOwner)
        return card.Controller == SourceCard.Controller;

      return card.Controller != SourceCard.Controller;
    }

    private void SetPowerIfModified(int? value)
    {
      if (_modifyPower == null)
        return;

      _powerModifier.Value = value;
    }

    private void SetToughnessIfModified(int? value)
    {
      if (_modifyToughness == null)
        return;

      _toughnessModifier.Value = value;
    }

    private void IncreasePowerIfModified(int? value)
    {
      if (_modifyPower == null)
        return;

      _powerModifier.Value += value;
    }

    private void IncreaseToughnessIfModified(int? value)
    {
      if (_modifyToughness == null)
        return;

      _toughnessModifier.Value += value;
    }

    protected override void Initialize()
    {
      _toughnessModifier.Initialize(ChangeTracker);
      _powerModifier.Initialize(ChangeTracker);

      Update();
    }

    protected override void Unapply()
    {
      if (_modifyPower.HasValue)
      {
        _strenght.RemovePowerModifier(_powerModifier);
      }

      if (_modifyToughness.HasValue)
      {
        _strenght.RemoveToughnessModifier(_toughnessModifier);
      }
    }

    private int GetPermanentCount()
    {
      var ctx = Ctx;

      if (_controlledBy == ControlledBy.SpellOwner)
      {
        return SourceCard.Controller.Battlefield.Count(c => _filter(c, ctx));
      }

      if (_controlledBy == ControlledBy.Opponent)
      {
        return SourceCard.Controller.Opponent.Battlefield.Count(c => _filter(c, ctx));
      }

      return Players.Permanents().Count(c => _filter(c, ctx));
    }

    private void Update()
    {
      var count = GetPermanentCount();

      SetPowerIfModified(count*_modifyPower);
      SetToughnessIfModified(count*_modifyToughness);
    }
  }
}