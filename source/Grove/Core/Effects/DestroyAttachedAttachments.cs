namespace Grove.Effects
{
  using System.Linq;

  public class DestroyAttachedAttachments : Effect
  {
    private readonly DynParam<Card> _permanent;
    private readonly CardSelector _filter;

    private DestroyAttachedAttachments() {}

    public DestroyAttachedAttachments(DynParam<Card> permanent, CardSelector filter = null)
    { 
      _permanent = permanent;
      _filter = filter ?? delegate { return true; };
      RegisterDynamicParameters(permanent);
    }

    protected override void ResolveEffect()
    {
      var attachments = _permanent.Value.Attachments
        .Where(x => _filter(x, Ctx))
        .ToList();

      foreach (var attachment in attachments)
      {
        attachment.Destroy();
      }
    }
  }
}