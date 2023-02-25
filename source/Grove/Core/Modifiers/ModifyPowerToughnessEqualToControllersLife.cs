namespace Grove.Modifiers
{
  using Grove.Events;
  using Grove.Infrastructure;

  public class ModifyPowerToughnessEqualToControllersLife : Modifier, IReceive<LifeChangedEvent>,
    IReceive<ControllerChangedEvent>, ICardModifier
  {
    private readonly IntegerSetter _strengthModifier = new IntegerSetter();
    private Strength _strength;

    public override void Apply(Strength strength)
    {
      _strength = strength;
      _strength.AddPowerModifier(_strengthModifier);
      _strength.AddToughnessModifier(_strengthModifier);
    }

    public void Receive(ControllerChangedEvent message)
    {
      _strengthModifier.Value = SourceCard.Controller.Life;
    }

    public void Receive(LifeChangedEvent message)
    {
      if (message.Player == SourceCard.Controller)
      {
        _strengthModifier.Value = SourceCard.Controller.Life;
      }
    }

    protected override void Unapply()
    {
      _strength.RemovePowerModifier(_strengthModifier);
      _strength.RemoveToughnessModifier(_strengthModifier);
    }

    protected override void Initialize()
    {
      _strengthModifier.Initialize(ChangeTracker);
      _strengthModifier.Value = SourceCard.Controller.Life;
    }
  }
}