namespace Grove.Core.Effects
{
  using System;
  using System.Linq;
  using Ai;

  public class ReturnAllPermanentsToHand : Effect
  {
    private readonly Func<Card, bool> _filter;

    private ReturnAllPermanentsToHand() {}

    public ReturnAllPermanentsToHand(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
      Category = EffectCategories.Bounce;
    }

    protected override void ResolveEffect()
    {
      foreach (var permanent in Players.Permanents().Where(card => _filter(card)).ToList())
      {
        permanent.PutToHand();
      }
    }
  }
}