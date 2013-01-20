namespace Grove.Core.Targeting
{
  using Core.Zones;

  public static class Zones
  {
    public static IsValidZone None()
    {
      return delegate { return false; };
    }

    public static IsValidZone OwnersHand()
    {
      return p => p.Zone == Zone.Hand && p.ZoneOwner == p.Source.Controller;
    }

    public static IsValidZone Battlefield()
    {
      return p => p.Zone == Zone.Battlefield;
    }

    public static IsValidZone Stack()
    {
      return p => p.Zone == Zone.Stack;
    }

    public static IsValidZone Graveyard()
    {
      return p => p.Zone == Zone.Graveyard;
    }

    public static IsValidZone YourGraveyard()
    {
      return p => p.Zone == Zone.Graveyard && p.ZoneOwner == p.Source.Controller;
    }

    public static IsValidZone BattlefieldOrStack()
    {
      return p => p.Zone == Zone.Stack || p.Zone == Zone.Battlefield;
    }
  }
}