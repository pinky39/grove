namespace Grove.Effects
{
  using System.Linq;
  using AI;

  public class PowderKegEffect : Effect
  {
    private int _countersCount;

    public PowderKegEffect()
    {
      SetTags(EffectTag.Destroy);
    }

    protected override void ResolveEffect()
    {
      var permanentsToDestroy = Players.Permanents()
        .Where(x => (x.Is().Artifact || x.Is().Creature) && x.ConvertedCost == _countersCount)
        .ToList();

      foreach (var card in permanentsToDestroy)
      {
        card.Destroy();
      }
    }

    protected override void Initialize()
    {
      _countersCount = Source.OwningCard
        .CountersCount(CounterType.Fuse);
    }
  }
}