namespace Grove.Effects
{
  public class PutOwnerToBattlefield : Effect
  {
    private readonly Zone _from;
    private readonly DynParam<bool> _tap;

    private PutOwnerToBattlefield() {}

    public PutOwnerToBattlefield(Zone @from, DynParam<bool> tap = null)
    {
      _from = @from;
      _tap = tap ?? false;
    }

    public override bool CanBeResolved()
    {
      return Source.OwningCard.Zone == _from && base.CanBeResolved();
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.PutToBattlefieldFrom(_from);

      if (_tap)
      {
        Source.OwningCard.Tap();
      }
    }
  }
}