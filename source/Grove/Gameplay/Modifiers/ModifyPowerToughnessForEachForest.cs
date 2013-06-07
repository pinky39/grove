namespace Grove.Gameplay.Modifiers
{
  using System;
  using System.Linq;
  using Characteristics;
  using Infrastructure;
  using Messages;
  using Zones;

  //public class ModifyPowerToughnessEqualToControllersLife : Modifier, IReceive<PlayerLifeChanged>, IReceive<ControllerChanged>
  //{
  //  private readonly IModifyPowerToughnessSpecification _spec;

  //  public ModifyPowerToughnessEqualToControllersLife(IModifyPowerToughnessSpecification how)
  //  {
  //    _spec = how;
  //  }

  //  protected override void Unapply()
  //  {
  //    throw new NotImplementedException();
  //  }

  //  public void Receive(PlayerLifeChanged message)
  //  {
  //    throw new NotImplementedException();
  //  }

  //  protected override void Initialize()
  //  {
  //    _spec.Initialize(Source.Controller.Life, Source.Controller.Life, ChangeTracker);
  //  }

  //  public void Receive(ControllerChanged message)
  //  {
  //    _spec.SetPower();
  //  }
  //}
  
  public class ModifyPowerToughnessForEachForest : Modifier, IReceive<ZoneChanged>, IReceive<ControllerChanged>
  {
    private readonly int? _modifyPower;    
    private readonly int? _modifyToughness;
    private Power _power;
    private Toughness _tougness;
    private IntegerModifier _toughnessModifier;
    private IntegerModifier _powerModifier;

    protected ModifyPowerToughnessForEachForest() {}

    public ModifyPowerToughnessForEachForest(int? power, int? toughness, Func<IntegerModifier> modifier)
    {
      _modifyPower = power;
      _modifyToughness = toughness;

      _toughnessModifier = modifier();
      _powerModifier = modifier();

    }

    public void Receive(ControllerChanged message)
    {
      if (message.Card.Is("forest") || message.Card == Source)
      {
        var forestCount = GetForestCount(Source.Controller);

        SetPowerIfModified(forestCount*_modifyPower);
        SetToughnessIfModified(forestCount*_modifyToughness);
      }
    }

    public void Receive(ZoneChanged message)
    {
      if (!IsForestControlledBySpellOwner(message.Card))
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


    public override void Apply(Power power)
    {
      if (_modifyPower.HasValue)
      {
        _power = power;
        power.AddModifier(_powerModifier);        
      }
    }

    public override void Apply(Toughness toughness)
    {
      if (_modifyToughness.HasValue)
      {
        _tougness = toughness;
        toughness.AddModifier(_toughnessModifier);        
      }
    }

    protected override void Initialize()
    {
      _toughnessModifier.Initialize(ChangeTracker);
      _powerModifier.Initialize(ChangeTracker);
      
      var forestCount = GetForestCount(Source.Controller);

      if (_modifyToughness.HasValue)
        _toughnessModifier.Value = forestCount* _modifyToughness;
      
      if (_modifyPower.HasValue)
        _powerModifier.Value = forestCount*_modifyPower;            
    }


    protected override void Unapply()
    {
      if (_modifyPower.HasValue)
      {
        _power.RemoveModifier(_powerModifier);
      }

      if (_modifyToughness.HasValue)
      {
        _tougness.RemoveModifier(_toughnessModifier);        
      }
    }

    private static int GetForestCount(Player player)
    {
      return player.Battlefield.Count(x => x.Is("forest"));
    }

    private bool IsForestControlledBySpellOwner(Card permanent)
    {
      return permanent.Is("forest") &&
        permanent.Controller == Source.Controller;
    }
  }
}