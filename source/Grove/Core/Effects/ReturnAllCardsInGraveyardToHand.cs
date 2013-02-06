namespace Grove.Core.Effects
{
  using System;
  using System.Linq;

  public class ReturnAllCardsInGraveyardToHand : Effect
  {
    private Func<Card, bool> Filter;

    private ReturnAllCardsInGraveyardToHand() {}

    public ReturnAllCardsInGraveyardToHand(Func<Card, bool> filter = null)
    {
      Filter = filter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      foreach (var permanent in Controller.Graveyard.Where(card => Filter(card)).ToList())
      {
        permanent.PutToHand();
      }
    }
  }
}