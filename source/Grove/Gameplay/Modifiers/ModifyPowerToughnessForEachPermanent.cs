namespace Grove.Gameplay.Modifiers
{
  using System;
  using System.Linq;
  using Characteristics;
  using Infrastructure;
  using Messages;
  using Zones;

  public class ModifyPowerToughnessForEachPermanent : Modifier, IReceive<ZoneChanged>, IReceive<ControllerChanged>,
    ICardModifier
  {
    private readonly int? _modifyPower;
    private readonly int? _modifyToughness;
    private readonly Func<Card, bool> _filter;
    private readonly IntegerModifier _powerModifier;
    private readonly IntegerModifier _toughnessModifier;
    private Strenght _strenght;

    protected ModifyPowerToughnessForEachPermanent() {}

    public ModifyPowerToughnessForEachPermanent(int? power, int? toughness, Func<Card, bool> filter,  Func<IntegerModifier> modifier)
    {
      _modifyPower = power;
      _modifyToughness = toughness;
      _filter = filter;

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

    public void Receive(ControllerChanged message)
    {
      if (message.Card.Is("forest") || message.Card == SourceCard)
      {
        var forestCount = GetPermanentCount(SourceCard.Controller);

        SetPowerIfModified(forestCount*_modifyPower);
        SetToughnessIfModified(forestCount*_modifyToughness);
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (!IsPermanentControlledBySpellOwner(message.Card))
        return;

      if (message.From == Zone.Battlefield)
      {
        IncreasePowerIfModified(-_modifyPower);
        IncreaseToughnessIfModified(-_modifyToughness);
      }

      else if (message.To == Zone.Battlefield)
      {
        IncreasePowerIfModified(_modifyPower);
        IncreaseToughnessIfModified(_modifyToughness);
      }
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

      var forestCount = GetPermanentCount(SourceCard.Controller);

      if (_modifyToughness.HasValue)
        _toughnessModifier.Value = forestCount*_modifyToughness;

      if (_modifyPower.HasValue)
        _powerModifier.Value = forestCount*_modifyPower;
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

    private int GetPermanentCount(Player player)
    {
      return player.Battlefield.Count(c => _filter(c));
    }

    private bool IsPermanentControlledBySpellOwner(Card permanent)
    {
      return _filter(permanent) &&
        permanent.Controller == SourceCard.Controller;
    }
  }
}