namespace Grove.Gameplay.Targeting
{
  using Gameplay.Zones;

  public class IsValidZoneParam
  {
    public Zone Zone { get; set; }
    public Player ZoneOwner { get; set; }
    public Card OwningCard { get; set; }
    public Player Controller { get; set; }
  }
}