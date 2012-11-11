namespace Grove.Core.Details.Cards.Effects
{
  using System;
  using System.Linq;

  public class ReturnAllCardsInGraveyardToHand : Effect
  {
    public Func<Card, bool> Filter = (card) => true;

    protected override void ResolveEffect()
    {
      foreach (var permanent in Controller.Graveyard.Where(card => Filter(card)).ToList())
      {
        permanent.ReturnToHand();
      }
    }
  }
}