namespace Grove.Gameplay.Modifiers
{
  using Characteristics;
  using Infrastructure;
  using Messages;

  public class ModifyPowerToughnessEqualToControllersLife : Modifier, IReceive<PlayerLifeChanged>,
    IReceive<ControllerChanged>, ICardModifier
  {
    private readonly IntegerSetter _strenghtModifier = new IntegerSetter();
    private Strenght _strenght;

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;
      _strenght.AddPowerModifier(_strenghtModifier);
      _strenght.AddToughnessModifier(_strenghtModifier);
    }

    public void Receive(ControllerChanged message)
    {
      _strenghtModifier.Value = SourceCard.Controller.Life;
    }

    public void Receive(PlayerLifeChanged message)
    {
      if (message.Player == SourceCard.Controller)
      {
        _strenghtModifier.Value = SourceCard.Controller.Life;
      }
    }

    protected override void Unapply()
    {
      _strenght.RemovePowerModifier(_strenghtModifier);
      _strenght.RemoveToughnessModifier(_strenghtModifier);
    }

    protected override void Initialize()
    {
      _strenghtModifier.Initialize(ChangeTracker);
      _strenghtModifier.Value = SourceCard.Controller.Life;
    }
  }
}