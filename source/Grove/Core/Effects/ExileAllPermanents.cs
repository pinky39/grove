namespace Grove.Effects
{
  using System;
  using System.Linq;
  using AI;

  public class ExileAllPermanents : Effect
  {
    private readonly Func<Effect, Card, bool> _filter;

    private ExileAllPermanents() {}

    public ExileAllPermanents(Func<Effect, Card, bool> filter = null)
    {
      _filter = filter;
      SetTags(EffectTag.Exile);
    }

    protected override void ResolveEffect()
    {
      var permanents = Players.Permanents().Where(c => _filter(this, c)).ToList();

      foreach (var permanent in permanents)
      {
        permanent.ExileFrom(Zone.Battlefield);
      }
    }
  }
}