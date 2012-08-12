namespace Grove.Core.Details.Cards.Modifiers
{
  using System.Linq;
  using Infrastructure;
  using Messages;
  using Zones;  
  
  public class Add11ForEachForest : Modifier, IReceive<CardChangedZone>
  {
    private Increment _increment;
    private Power _power;
    private Toughness _tougness;

    public void Receive(CardChangedZone message)
    {
      if (!IsForestControlledBySpellOwner(message.Card))
        return;

      if (message.From == Zone.Battlefield)
      {
        _increment--;
      }

      else if (message.To == Zone.Battlefield)
      {
        _increment++;
      }
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
      _increment = new Increment(
        GetForestCount(Source.Controller), ChangeTracker);
    }

    protected override void Unapply()
    {
      _power.RemoveModifier(_increment);
      _tougness.RemoveModifier(_increment);
    }

    private static int GetForestCount(Player player)
    {
      return player.Battlefield.Count(x => x.Is("forest"));
    }

    private bool IsForestControlledBySpellOwner(Card permanent)
    {
      return permanent.Is("forest") &&
        permanent.Controller == Source.Controller;
    }
  }
}