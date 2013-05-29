namespace Grove.Gameplay.Effects
{
  public class PutIntoPlay : Effect
  {
    private readonly DynParam<bool> _tap;

    private PutIntoPlay() {}

    public PutIntoPlay(DynParam<bool> tap = null)
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