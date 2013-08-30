namespace Grove.Gameplay.Effects
{
  using System.Linq;

  public class DestroyAttachedAuras : Effect
  {
    private readonly DynParam<Card> _permanent;

    private DestroyAttachedAuras() {}

    public DestroyAttachedAuras(DynParam<Card> permanent)
    {
      _permanent = permanent;
      RegisterDynamicParameters(permanent);
    }

    protected override void ResolveEffect()
    {
      var auras = _permanent.Value.Attachments
        .Where(x => x.Is().Aura).ToList();

      foreach (var aura in auras)
      {
        aura.Destroy();
      }
    }
  }
}