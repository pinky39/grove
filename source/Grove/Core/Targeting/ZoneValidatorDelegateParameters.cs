namespace Grove.Core.Targeting
{
  using Core.Zones;

  public class ZoneValidatorDelegateParameters : GameObject
  {
    private ZoneValidatorDelegateParameters() {}

    public ZoneValidatorDelegateParameters(Zone zone, Player zoneOwner, Player controller, Game game,
      Card owningCard = null)
    {
      Zone = zone;
      OwningCard = owningCard;
      ZoneOwner = zoneOwner;
      Controller = controller;
      Game = game;
    }

    public Zone Zone { get; private set; }
    public Player ZoneOwner { get; private set; }
    public Card OwningCard { get; private set; }
    public Player Controller { get; private set; }
  }
}