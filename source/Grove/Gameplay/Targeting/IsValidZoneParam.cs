namespace Grove.Gameplay.Targeting
{
  using Card;
  using Gameplay.Zones;
  using Player;

  public class IsValidZoneParam
  {
    public Zone Zone { get; set; }
    public Player ZoneOwner { get; set; }
    public Card OwningCard { get; set; }
    public Player Controller { get; set; }
  }
}