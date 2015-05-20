namespace Grove
{
  using System;

  public class IsValidZoneBuilder
  {
    private readonly IsValidTargetBuilder _p;
    public Func<IsValidZoneParameters, bool> IsValidZone { get; private set; }
    public bool MustBeTargetable { get; private set; }

    public IsValidZoneBuilder(IsValidTargetBuilder parent)
    {
      _p = parent;
    }

    public IsValidZoneBuilder In { get { return this; } }
    public IsValidZoneBuilder On { get { return this; } }

    public IsValidTargetBuilder OwnersHand()
    {
      IsValidZone = p => p.Zone == Zone.Hand && p.ZoneOwner == p.Controller;
      MustBeTargetable = false;
      return _p;
    }

    public IsValidTargetBuilder Battlefield()
    {
      IsValidZone = p => p.Zone == Zone.Battlefield;
      MustBeTargetable = true;
      return _p;
    }

    public IsValidTargetBuilder Stack()
    {
      IsValidZone = p => p.Zone == Zone.Stack;
      MustBeTargetable = false;
      return _p;
    }

    public IsValidTargetBuilder Graveyard()
    {
      IsValidZone = p => p.Zone == Zone.Graveyard;
      MustBeTargetable = false;
      return _p;
    }

    public IsValidTargetBuilder YourGraveyard()
    {
      IsValidZone = p => p.Zone == Zone.Graveyard && p.ZoneOwner == p.Controller;
      MustBeTargetable = false;
      return _p;
    }

    public IsValidTargetBuilder BattlefieldOrStack()
    {
      IsValidZone = p => p.Zone == Zone.Stack || p.Zone == Zone.Battlefield;
      MustBeTargetable = true;
      return _p;
    }
  }
}