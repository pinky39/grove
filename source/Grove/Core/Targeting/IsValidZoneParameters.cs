namespace Grove
{
  public class IsValidZoneParameters
  {
    public readonly Zone Zone;
    public readonly Player ZoneOwner;    
    public readonly Player Controller;
    
    public IsValidZoneParameters(Zone zone, Player zoneOwner, Player controller)
    {
      Zone = zone;
      ZoneOwner = zoneOwner;      
      Controller = controller;
    }
  }
}