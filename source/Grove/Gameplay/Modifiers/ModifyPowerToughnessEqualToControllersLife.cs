namespace Grove.Gameplay.Modifiers
{
  using Characteristics;
  using Infrastructure;
  using Messages;

  public class ModifyPowerToughnessEqualToControllersLife : Modifier, IReceive<PlayerLifeChanged>, IReceive<ControllerChanged>, ICardModifier
  {    
    private readonly IntegerSetter _strenghtModifier = new IntegerSetter();
    private Power _power;
    private Toughness _toughness;

    public override void Apply(Power power)
    {
      _power = power;
      power.AddModifier(_strenghtModifier);
    }

    public override void Apply(Toughness toughness)
    {
      _toughness = toughness;
      _toughness.AddModifier(_strenghtModifier);
    }
    
    protected override void Unapply()
    {
      _toughness.RemoveModifier(_strenghtModifier);
      _power.RemoveModifier(_strenghtModifier);
    }

    public void Receive(PlayerLifeChanged message)
    {
      if (message.Player == SourceCard.Controller)
      {
        _strenghtModifier.Value = SourceCard.Controller.Life;
      }
    }

    protected override void Initialize()
    {
      _strenghtModifier.Initialize(ChangeTracker);
      _strenghtModifier.Value = SourceCard.Controller.Life;
    }

    public void Receive(ControllerChanged message)
    {
      _strenghtModifier.Value = SourceCard.Controller.Life;
    }
  }
}