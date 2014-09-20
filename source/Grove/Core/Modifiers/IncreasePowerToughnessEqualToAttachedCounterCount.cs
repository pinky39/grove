namespace Grove.Modifiers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class IncreasePowerToughnessEqualToAttachedCounterCount : Modifier,
    IReceive<CounterAddedEvent>, IReceive<CounterRemovedEvent>, ICardModifier
  {
    private readonly CounterType _counterType;
    private readonly IntegerIncrement _strenghtModifier = new IntegerIncrement();
    private Strenght _strenght;

    private IncreasePowerToughnessEqualToAttachedCounterCount() {}

    public IncreasePowerToughnessEqualToAttachedCounterCount(CounterType counterType)
    {
      _counterType = counterType;      
    }

    public void Receive(CounterAddedEvent message)
    {
      if (OwningCard.Attachments.Contains(message.OwningCard) && message.Counter.Type == _counterType)
      {
        _strenghtModifier.Value++;
      }
    }

    public void Receive(CounterRemovedEvent message)
    {
      if (OwningCard.Attachments.Contains(message.OwningCard) && message.Counter.Type == _counterType)
      {
        _strenghtModifier.Value--;
      }
    }

    public override void Apply(Strenght strenght)
    {
      _strenght = strenght;
      _strenght.AddPowerModifier(_strenghtModifier);
      _strenght.AddToughnessModifier(_strenghtModifier);
    }

    protected override void Initialize()
    {
      _strenghtModifier.Initialize(ChangeTracker);
      _strenghtModifier.Value = OwningCard.CountersCount(_counterType);
    }

    protected override void Unapply()
    {
      _strenght.RemovePowerModifier(_strenghtModifier);
      _strenght.RemoveToughnessModifier(_strenghtModifier);
    }
  }
}