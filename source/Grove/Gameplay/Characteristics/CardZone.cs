namespace Grove.Gameplay.Characteristics
{
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

    public void ChangeZone(IZone newZone)
    {
      var oldZone = _current.Value;
      _current.Value = newZone;

      oldZone.Remove(_card);

      if (oldZone.Zone != newZone.Zone)
      {
        Publish(new ZoneChanged
          {
            Card = _card,
            From = oldZone.Zone,
            To = newZone.Zone
          });


        oldZone.AfterRemove(_card);        
        newZone.AfterAdd(_card);
      }
    }
  }
}