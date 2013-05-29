namespace Grove.Gameplay.Modifiers
{
  using System.Linq;
  using Characteristics;
  using Infrastructure;
  using Messages;
  using Zones;

  public class Add11ForEachOtherCreature : Modifier, IReceive<ZoneChanged>,
    IReceive<TypeChanged>, IReceive<ControllerChanged>
  {
    private Increment _increment = new Increment(0);
    private Power _power;
    private Toughness _tougness;

    public void Receive(ControllerChanged message)
    {
      if (message.Card.Is().Creature || message.Card == Source)
      {
        _increment.Value = GetCreatureCount();
      }
    }

    public void Receive(TypeChanged message)
    {
      if (message.Card.Controller != Source.Controller ||
        !message.Card.Is().Creature)
      {
        return;
      }

      _increment.Value = GetCreatureCount();
    }

    public void Receive(ZoneChanged message)
    {
      if (message.Card.Controller != Source.Controller ||
        !message.Card.Is().Creature)
      {
        return;
      }

      if (message.From == Zone.Battlefield)
      {
        _increment--;
      }

      else if (message.To == Zone.Battlefield)
      {
        _increment++;
      }
    }

    protected override void Unapply()
    {
      _power.RemoveModifier(_increment);
      _tougness.RemoveModifier(_increment);
    }

    public override void Apply(Power power)
    {
      _power = power;
      power.AddModifier(_increment);
    }

    public override void Apply(Toughness toughness)
    {
      _tougness = toughness;
      toughness.AddModifier(_increment);
    }

    protected override void Initialize()
    {
      _increment.Initialize(ChangeTracker);
      _increment.Value = GetCreatureCount();
    }

    private int GetCreatureCount()
    {
      return Source.Controller.Battlefield.Count(card => card != Target && card.Is().Creature);
    }
  }
}