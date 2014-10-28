namespace Grove.Effects
{
  // This effect should be used only when casting permanents
  // from hand. It is a kind of dummy effect, cause permanenets
  // are put to play by the CastRule.
  public class CastPermanent : Effect
  {
    private readonly DynParam<bool> _tap;

    private CastPermanent() {}

    public CastPermanent(DynParam<bool> tap = null)
    {
      _tap = tap ?? false;
      RegisterDynamicParameters(tap);
    }

    protected override void ResolveEffect()
    {
      if (_tap.Value)
      {
        Source.OwningCard.Tap();
      }
    }
  }
}