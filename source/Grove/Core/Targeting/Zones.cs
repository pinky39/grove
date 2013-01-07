namespace Grove.Core.Targeting
{
  using Core.Zones;

  public static class Zones
  {
    public static ZoneValidatorDelegate None()
    {
      return delegate { return false; };
    }

    public static ZoneValidatorDelegate OwnersHand()
    {
      return p => p.Zone == Zone.Hand && p.ZoneOwner == p.Source.Controller;
    }

    public static ZoneValidatorDelegate Battlefield()
    {
      return p => p.Zone == Zone.Battlefield;
    }

    public static ZoneValidatorDelegate Stack()
    {
      return p => p.Zone == Zone.Stack;
    }

    public static ZoneValidatorDelegate Graveyard()
    {
      return p => p.Zone == Zone.Graveyard;
    }

    public static ZoneValidatorDelegate YourGraveyard()
    {
      return p => p.Zone == Zone.Graveyard && p.ZoneOwner == p.Source.Controller;
    }

    public static ZoneValidatorDelegate BattlefieldOrStack()
    {
      return p => p.Zone == Zone.Stack || p.Zone == Zone.Battlefield;
    }
  }
}