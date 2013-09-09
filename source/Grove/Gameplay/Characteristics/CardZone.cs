namespace Grove.Gameplay.Characteristics
{
  using System;
  using Infrastructure;
  using Messages;
  using Misc;
  using Zones;

  [Copyable]
  public class CardZone : GameObject, IHashable
  {
    private readonly Trackable<IZone> _current = new Trackable<IZone>(new NullZone());
    private Card _card;

    public Zone Current { get { return _current.Value.Zone; } }

    public int CalculateHash(HashCalculator calc)
    {
      return Current.GetHashCode();
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;
      _card = card;

      _current.Initialize(ChangeTracker, card);
    }

    public void ChangeZoneTo(IZone zone, Action<Card> onChange, Action<Card> onNoChange)
    {
      var oldZone = _current.Value;      

      if (oldZone == zone)
      {
        onNoChange(_card);
        return;
      }
      
      oldZone.Remove(_card);
      onChange(_card);
      
      _current.Value = zone;

      if (oldZone.Zone == zone.Zone)
      {
        // change of controller
        return;
      }

      Publish(new ZoneChanged
        {
          Card = _card,
          From = oldZone.Zone,
          To = zone.Zone
        });


      oldZone.AfterRemove(_card);
      zone.AfterAdd(_card);
    }
  }
}