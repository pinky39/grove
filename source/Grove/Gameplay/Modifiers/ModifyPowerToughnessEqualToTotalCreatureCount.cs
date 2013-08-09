namespace Grove.Gameplay.Modifiers
{
  using System.Linq;
  using Characteristics;
  using Infrastructure;
  using Messages;

  public class ModifyPowerToughnessEqualToTotalCreatureCount : Modifier, IReceive<ZoneChanged>, IReceive<TypeChanged>, ICardModifier
  {
    private readonly IntegerSetter _strenghtModifier = new IntegerSetter();
    private Strenght _strenght;

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;
      _strenght.AddPowerModifier(_strenghtModifier);
      _strenght.AddToughnessModifier(_strenghtModifier);
    }
    
    protected override void Unapply()
    {
      _strenght.RemovePowerModifier(_strenghtModifier);
      _strenght.RemoveToughnessModifier(_strenghtModifier);
    }

    public void Receive(ZoneChanged message)
    {
      if (!message.Card.Is().Creature)
        return;
      
      if (message.ToBattlefield)
      {
        _strenghtModifier.Value++;
        return;
      }

      if (message.FromBattlefield)
      {
        _strenghtModifier.Value--;
      }      
    }

    protected override void Initialize()
    {
      _strenghtModifier.Initialize(ChangeTracker);
      _strenghtModifier.Value = Players.Permanents().Count(x => x.Is().Creature);
    }

    public void Receive(TypeChanged message)
    {
      if (message.OldValue.Creature && !message.NewValue.Creature)
      {
        _strenghtModifier.Value--;
        return;
      }

      if (!message.OldValue.Creature && message.NewValue.Creature)
      {
        _strenghtModifier.Value++;
        return;
      }
    }
  }
}