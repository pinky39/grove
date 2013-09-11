namespace Grove.Gameplay.Effects
{
  using Artifical;

  public class DestroyPermanent : Effect
  {
    private readonly DynParam<Card> _permanent;

    private DestroyPermanent() {}

    public DestroyPermanent(DynParam<Card> permanent)
    {
      _permanent = permanent;
      Kinds = EffectTag.Destroy;

      RegisterDynamicParameters(permanent);
    }


    protected override void ResolveEffect()
    {
      _permanent.Value.Destroy();
    }
  }
}