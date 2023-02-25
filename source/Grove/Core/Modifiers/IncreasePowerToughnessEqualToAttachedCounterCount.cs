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
    private readonly IntegerIncrement _strengthModifier = new IntegerIncrement();
    private Strength _strength;

    private IncreasePowerToughnessEqualToAttachedCounterCount() { }

    public IncreasePowerToughnessEqualToAttachedCounterCount(CounterType counterType)
    {
      _counterType = counterType;
    }

    public void Receive(CounterAddedEvent message)
    {
      if (OwningCard.Attachments.Contains(message.OwningCard) && message.Counter.Type == _counterType)
      {
        _strengthModifier.Value++;
      }
    }

    public void Receive(CounterRemovedEvent message)
    {
      if (OwningCard.Attachments.Contains(message.OwningCard) && message.Counter.Type == _counterType)
      {
        _strengthModifier.Value--;
      }
    }

    public override void Apply(Strength strength)
    {
      _strength = strength;
      _strength.AddPowerModifier(_strengthModifier);
      _strength.AddToughnessModifier(_strengthModifier);
    }

    protected override void Initialize()
    {
      _strengthModifier.Initialize(ChangeTracker);
      _strengthModifier.Value = OwningCard.CountersCount(_counterType);
    }

    protected override void Unapply()
    {
      _strength.RemovePowerModifier(_strengthModifier);
      _strength.RemoveToughnessModifier(_strengthModifier);
    }
  }
}