namespace Grove.Gameplay.Targeting
{
  using Gameplay.Zones;

  public class IsValidZoneParam
  {
    public readonly Zone Zone;
    public readonly Player ZoneOwner;
    public readonly Card OwningCard;
    public readonly Player Controller;
    
    public IsValidZoneParam(Zone zone, Player zoneOwner, Card owningCard, Player controller)
    {
      Zone = zone;
      ZoneOwner = zoneOwner;
      OwningCard = owningCard;
      Controller = controller;
    }
  }
}