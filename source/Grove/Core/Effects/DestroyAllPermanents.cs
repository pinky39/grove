namespace Grove.Effects
{
  using System.Linq;
  using AI;

  public class DestroyAllPermanents : Effect
  {
    private readonly bool _allowToRegenerate;
    private readonly CardSelector _filter;

    private DestroyAllPermanents() {}

    public DestroyAllPermanents(
      CardSelector filter = null,
      bool allowToRegenerate = true)
    {      
      _allowToRegenerate = allowToRegenerate;
      _filter = filter;

      SetTags(EffectTag.Destroy);

      if (!allowToRegenerate)
        SetTags(EffectTag.CannotRegenerate);
    }

    protected override void ResolveEffect()
    {
      var permanents = Players.Permanents()
        .Where(c => _filter(c, Ctx))
        .ToList();

      foreach (var permanent in permanents)
      {
        permanent.Destroy(_allowToRegenerate);
      }
    }
  }
}