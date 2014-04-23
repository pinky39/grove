namespace Grove
{
  public class IsValidZoneParam
  {
    public readonly Zone Zone;
    public readonly Player ZoneOwner;    
    public readonly Player Controller;
    
    public IsValidZoneParam(Zone zone, Player zoneOwner, Player controller)
    {
      Zone = zone;
      ZoneOwner = zoneOwner;      
      Controller = controller;
    }
  }
}