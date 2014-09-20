namespace Grove.Modifiers
{
  using System.Linq;
  using Events;
  using Infrastructure;

  public class ModifyPowerToughnessEqualToTotalHandsCount : Modifier, IReceive<ZoneChangedEvent>,
    ICardModifier
  {
    private readonly IntegerSetter _strenghtModifier = new IntegerSetter();
    private Strenght _strenght;

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;
      _strenght.AddPowerModifier(_strenghtModifier);
      _strenght.AddToughnessModifier(_strenghtModifier);
    }

    public void Receive(ZoneChangedEvent message)
    {
      if (message.ToHand)
      {
        _strenghtModifier.Value++;
        return;
      }

      if (message.FromHand)
      {
        _strenghtModifier.Value--;
        return;
      }
    }

    protected override void Initialize()
    {
      _strenghtModifier.Initialize(ChangeTracker);
      _strenghtModifier.Value = Players.Sum(x => x.Hand.Count);
    }

    protected override void Unapply()
    {
      _strenght.RemovePowerModifier(_strenghtModifier);
      _strenght.RemoveToughnessModifier(_strenghtModifier);
    }
  }
}