namespace Grove.Modifiers
{
  using System.Linq;
  using Events;
  using Infrastructure;

  public class ModifyPowerToughnessEqualToTotalHandsCount : Modifier, IReceive<ZoneChangedEvent>,
    ICardModifier
  {
    private readonly IntegerSetter _strengthModifier = new IntegerSetter();
    private Strength _strength;

    public override void Apply(Strength strength)
    {
      _strength = strength;
      _strength.AddPowerModifier(_strengthModifier);
      _strength.AddToughnessModifier(_strengthModifier);
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (message.ToHand)
      {
        _strengthModifier.Value++;
        return;
      }

      if (message.FromHand)
      {
        _strengthModifier.Value--;
        return;
      }
    }

    protected override void Initialize()
    {
      _strengthModifier.Initialize(ChangeTracker);
      _strengthModifier.Value = Players.Sum(x => x.Hand.Count);
    }

    protected override void Unapply()
    {
      _strength.RemovePowerModifier(_strengthModifier);
      _strength.RemoveToughnessModifier(_strengthModifier);
    }
  }
}