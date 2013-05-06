namespace Grove.Gameplay.Effects
{
  using System;
  using System.Linq;

  public class ReturnAllCardsInGraveyardToHand : Effect
  {
    private readonly Func<Card, bool> _filter;

    private ReturnAllCardsInGraveyardToHand() {}

    public ReturnAllCardsInGraveyardToHand(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      foreach (var permanent in Controller.Graveyard.Where(card => _filter(card)).ToList())
      {
        permanent.PutToHand();
      }
    }
  }
}