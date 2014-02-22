namespace Grove.Gameplay
{
  public static class ZoneHelpers
  {
    public static bool IsHiddenZone(this Zone zone)
    {
      return zone == Zone.Library || zone == Zone.Hand;
    }
  }
}