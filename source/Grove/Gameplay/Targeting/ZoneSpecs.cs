namespace Grove.Gameplay.Targeting
{
  using Gameplay.Zones;

  public class ZoneSpecs
  {
    private readonly TargetValidatorParameters _p;

    public ZoneSpecs(TargetValidatorParameters p)
    {
      _p = p;
    }

    public TargetValidatorParameters None()
    {
      _p.IsValidZone = delegate { return false; };
      return _p;
    }

    public TargetValidatorParameters OwnersHand()
    {
      _p.IsValidZone = p => p.Zone == Zone.Hand && p.ZoneOwner == p.Controller;
      _p.MustBeTargetable = false;
      return _p;
    }

    public TargetValidatorParameters Battlefield()
    {
      _p.IsValidZone = p => p.Zone == Zone.Battlefield;
      return _p;
    }

    public TargetValidatorParameters Stack()
    {
      _p.IsValidZone = p =>
        {
          return p.Zone == Zone.Stack;
        };
      _p.MustBeTargetable = false;
      return _p;
    }

    public TargetValidatorParameters Graveyard()
    {
      _p.IsValidZone = p => p.Zone == Zone.Graveyard;
      _p.MustBeTargetable = false;
      return _p;
    }

    public TargetValidatorParameters YourGraveyard()
    {
      _p.IsValidZone = p => p.Zone == Zone.Graveyard && p.ZoneOwner == p.Controller;
      _p.MustBeTargetable = false;
      return _p;
    }

    public TargetValidatorParameters BattlefieldOrStack()
    {
      _p.IsValidZone = p => p.Zone == Zone.Stack || p.Zone == Zone.Battlefield;
      return _p;
    }
  }
}