namespace Grove.Gameplay.Modifiers
{
  using System.Linq;
  using Characteristics;
  using Infrastructure;
  using Messages;
  using Zones;

  public class Add11ForEachOtherCreature : Modifier, ICardModifier, IReceive<ZoneChanged>,
    IReceive<TypeChanged>, IReceive<ControllerChanged>
  {
    private IntegerIncrement _integerIncrement = new IntegerIncrement();
    private Power _power;
    private Toughness _tougness;

    public void Receive(ControllerChanged message)
    {
      if (message.Card.Is().Creature || message.Card == SourceCard)
      {
        _integerIncrement.Value = GetCreatureCount();
      }
    }

    public void Receive(TypeChanged message)
    {
      if (message.Card.Controller != SourceCard.Controller ||
        !message.Card.Is().Creature)
      {
        return;
      }

      _integerIncrement.Value = GetCreatureCount();
    }

    public void Receive(ZoneChanged message)
    {
      if (message.Card.Controller != SourceCard.Controller ||
        !message.Card.Is().Creature)
      {
        return;
      }

      if (message.From == Zone.Battlefield)
      {
        _integerIncrement--;
      }

      else if (message.To == Zone.Battlefield)
      {
        _integerIncrement++;
      }
    }

    protected override void Unapply()
    {
      _power.RemoveModifier(_integerIncrement);
      _tougness.RemoveModifier(_integerIncrement);
    }

    public override void Apply(Power power)
    {
      _power = power;
      power.AddModifier(_integerIncrement);
    }

    public override void Apply(Toughness toughness)
    {
      _tougness = toughness;
      toughness.AddModifier(_integerIncrement);
    }

    protected override void Initialize()
    {
      _integerIncrement.Initialize(ChangeTracker);
      _integerIncrement.Value = GetCreatureCount();
    }

    private int GetCreatureCount()
    {
      return SourceCard.Controller.Battlefield.Count(card => card != Owner && card.Is().Creature);
    }
  }
}