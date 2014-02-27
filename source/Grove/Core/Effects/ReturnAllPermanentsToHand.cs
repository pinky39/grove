namespace Grove.Effects
{
  using System;
  using System.Linq;
  using Grove.AI;

  public class ReturnAllPermanentsToHand : Effect
  {
    private readonly Func<Effect, Card, bool> _filter;

    private ReturnAllPermanentsToHand() {}

    public ReturnAllPermanentsToHand(Func<Card, bool> filter) : this((e, c) => filter(c)) {}

    public ReturnAllPermanentsToHand(Func<Effect, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
      SetTags(EffectTag.Bounce);
    }

    protected override void ResolveEffect()
    {
      foreach (var permanent in Players.Permanents().Where(card => _filter(this, card)).ToList())
      {
        permanent.PutToHand();
      }
    }
  }
}